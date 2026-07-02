using Godot;
using System.Collections.Generic;
using SlaughtfulConquest.Enemies;
using SlaughtfulConquest.Enemies.Rex;

namespace SlaughtfulConquest.Maps.Test;

public partial class TestMap : Map
{
	public PackedScene TrikeScene { get; set; }
	public PackedScene BrontoScene { get; set; }
	public PackedScene RexScene { get; set; }
	public float SpawnInterval { get; set; } = 1.5f;

	private readonly List<WaveDefinition> _waves = [];
	private int _currentWaveIndex = -1;
	private int _startingHealth;
	private int _aliveThisWave;
	private Queue<PackedScene> _spawnQueue = new();
	private Timer _spawnTimer;

	private RichTextLabel _waveLabel;

	public override void _Ready()
	{
		base._Ready();
		Health = 1000;
		Bones = 40;
		_startingHealth = Health;

		TrikeScene = ResourceLoader.Load<PackedScene>("uid://b7sbvxx32g813");
		BrontoScene = ResourceLoader.Load<PackedScene>("uid://bbby3rywvgjvk");
		RexScene = ResourceLoader.Load<PackedScene>("uid://bjg04vfoiqwgh");
		_waveLabel = GetNode<RichTextLabel>("WaveLabel");

		_spawnTimer = new Timer { WaitTime = SpawnInterval, OneShot = false };
		AddChild(_spawnTimer);
		_spawnTimer.Timeout += OnSpawnTick;

		BuildWaves();
		StartNextWave();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		base._UnhandledInput(@event);
		
		if (@event.IsActionPressed("pause"))
		{
			Globals.Ui.Open("uid://dy1l22h3u34at");
		}
	}

	private void BuildWaves()
	{
		_waves.Add(new WaveDefinition
		{
			EnemyCounts = new() { { TrikeScene, 10 } }
		});

		_waves.Add(new WaveDefinition
		{
			EnemyCounts = new() { { TrikeScene, 8 }, { BrontoScene, 4 } }
		});

		_waves.Add(new WaveDefinition
		{
			EnemyCounts = new() { { TrikeScene, 6 }, { BrontoScene, 6 }, { RexScene, 2 } }
		});

		_waves.Add(new WaveDefinition
		{
			EnemyCounts = new() { { BrontoScene, 8 }, { RexScene, 6 } },
			IsSkippableIfUndamaged = true
		});

		_waves.Add(new WaveDefinition
		{
			EnemyCounts = new() { { RexScene, 12 } },
			IsBossWave = true
		});
	}

	private void StartNextWave()
	{
		_currentWaveIndex++;

		if (_currentWaveIndex >= _waves.Count)
		{
			GD.Print("All waves complete!");
			GetTree().Quit();
			return;
		}

		WaveDefinition wave = _waves[_currentWaveIndex];

		if (wave.IsSkippableIfUndamaged && Health == _startingHealth)
		{
			GD.Print($"Wave {_currentWaveIndex + 1} skipped — no damage taken yet.");
			StartNextWave();
			return;
		}

		GD.Print($"Starting wave {_currentWaveIndex + 1}");
		_waveLabel.Text = "Wave: " + (_currentWaveIndex + 1);
		

		_spawnQueue = new Queue<PackedScene>(FlattenWave(wave));
		_aliveThisWave = 0;
		_spawnTimer.Start();
	}

	private static IEnumerable<PackedScene> FlattenWave(WaveDefinition wave)
	{
		foreach (var kvp in wave.EnemyCounts)
		{
			for (int i = 0; i < kvp.Value; i++)
				yield return kvp.Key;
		}
	}

	private void OnSpawnTick()
	{
		if (_spawnQueue.Count == 0)
		{
			_spawnTimer.Stop();
			return;
		}

		PackedScene scene = _spawnQueue.Dequeue();
		SpawnEnemy(scene);

		if (_spawnQueue.Count == 0)
			_spawnTimer.Stop();
	}

	private void SpawnEnemy(PackedScene scene)
	{
		if (scene == null) return;
		if (scene.Instantiate() is not Enemy enemy) return;

		EnemyPath.AddChild(enemy);
		_aliveThisWave++;

		enemy.TreeExited += OnEnemyRemoved;
	}

	private void OnEnemyRemoved()
	{
		_aliveThisWave--;
		CheckWaveCompletion();
	}

	private void CheckWaveCompletion()
	{
		if (_spawnQueue.Count > 0)
		{
		}
		if (_aliveThisWave > 0) return;

		StartNextWave();
	}
}

public class WaveDefinition
{
	public Dictionary<PackedScene, int> EnemyCounts { get; set; } = new();
	public bool IsSkippableIfUndamaged { get; set; }
	public bool IsBossWave { get; set; }
}
