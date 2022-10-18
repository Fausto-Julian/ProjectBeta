using System;
using UnityEngine;
using Fusion;
using UI = UnityEngine.UI;
using Stats = Fusion.Simulation.Statistics;
using Fusion.StatsInternal;

[ScriptHelp(BackColor = EditorHeaderBackColor.Olive)]
public abstract class FusionGraphBase : Fusion.Behaviour, IFusionStatsView {

  protected const int PAD = FusionStatsUtilities.PAD;
  protected const int MRGN = FusionStatsUtilities.MARGIN;
  protected const int MAX_FONT_SIZE_WITH_GRAPH = 24;

  [SerializeField] [HideInInspector] protected UI.Text  LabelTitle;
  [SerializeField] [HideInInspector] protected UI.Image BackImage;

  /// <summary>
  /// Which section of the Fusion engine is being monitored. In combination with StatId, this selects the stat being monitored.
  /// </summary>
  [InlineHelp]
  [SerializeField]
  protected Simulation.Statistics.StatSourceTypes _statSourceType;
  public Simulation.Statistics.StatSourceTypes StateSourceType {
    get => _statSourceType;
    set {
      _statSourceType = value;
      TryConnect();
    }
  }

  /// <summary>
  /// The specific stat being monitored.
  /// </summary>
  [InlineHelp]
  [SerializeField]
  [CastEnum(nameof(CastToStatType))]
  protected int _statId;
  public int StatId {
    get => _statId;
    set {
      _statId = value;
      TryConnect();
    }
  }

  [InlineHelp]
  public float WarnThreshold;

  [InlineHelp]
  public float ErrorThreshold;

  protected IStatsBuffer _statsBuffer;
  public IStatsBuffer StatsBuffer {
    get {
      if (_statsBuffer == null) {
        TryConnect();
      }
      return _statsBuffer;
    }
  }

  protected bool _isOverlay;
  public bool IsOverlay {
    set {
      if (_isOverlay != value) {
        _isOverlay = value;
        CalculateLayout();
        _layoutDirty = true;
      }
    }
    get {
      return _isOverlay;
    }
  }



  protected virtual Color BackColor {
    get {
      if (_statSourceType == Simulation.Statistics.StatSourceTypes.Simulation) {
        return _fusionStats.SimDataBackColor;
      }
      if (_statSourceType == Simulation.Statistics.StatSourceTypes.NetConnection) {
        return _fusionStats.NetDataBackColor;
      }
      return _fusionStats.ObjDataBackColor;
    }
  }

  protected Type CastToStatType => 
    (_statSourceType == Simulation.Statistics.StatSourceTypes.Simulation)    ? typeof(Simulation.Statistics.SimStats) : 
    (_statSourceType == Simulation.Statistics.StatSourceTypes.NetConnection) ? typeof(Simulation.Statistics.NetStats) : 
                                                               typeof(Simulation.Statistics.ObjStats);

  protected FusionStats _fusionStats;
  protected FusionStats LocateParentFusionStats() {
    if (_fusionStats == null) {
      _fusionStats = GetComponentInParent<FusionStats>();
    }
    return _fusionStats;
  }

  protected bool _layoutDirty = true;

  protected Simulation.Statistics.StatsPer CurrentPer;

  public Simulation.Statistics.StatSourceInfo StatSourceInfo;


  // Track source values to detect changes in OnValidate.
  [SerializeField]
  [HideInInspector]
    Simulation.Statistics.StatSourceTypes _prevStatSourceType;
  
  [SerializeField]
  [HideInInspector]
  int                   _prevStatId;

#if UNITY_EDITOR

  protected virtual void OnValidate() {
    if (_statSourceType != _prevStatSourceType || _statId != _prevStatId) {
      WarnThreshold = 0;
      ErrorThreshold = 0;
      _prevStatSourceType = _statSourceType;
      _prevStatId = _statId;
    }
  }
#endif

  public virtual void Initialize() {

  }

  public virtual void CyclePer() {

    var flags = StatSourceInfo.PerFlags;
    switch (CurrentPer) {
      case Simulation.Statistics.StatsPer.Individual:
        if ((flags & Simulation.Statistics.StatsPer.Tick) == Simulation.Statistics.StatsPer.Tick) {
                    CurrentPer = Simulation.Statistics.StatsPer.Tick;
        } else if ((flags & Simulation.Statistics.StatsPer.Second) == Simulation.Statistics.StatsPer.Second) {
                    CurrentPer = Simulation.Statistics.StatsPer.Second;
        }
        return;

      case Simulation.Statistics.StatsPer.Tick:
        if ((flags & Simulation.Statistics.StatsPer.Second) == Simulation.Statistics.StatsPer.Second) {
                    CurrentPer = Simulation.Statistics.StatsPer.Second;
        } else if ((flags & Simulation.Statistics.StatsPer.Individual) == Simulation.Statistics.StatsPer.Individual) {
                    CurrentPer = Simulation.Statistics.StatsPer.Individual;
        }
        return;

      case Simulation.Statistics.StatsPer.Second:
        if ((flags & Simulation.Statistics.StatsPer.Individual) == Simulation.Statistics.StatsPer.Individual) {
                    CurrentPer = Simulation.Statistics.StatsPer.Individual;
        } else if ((flags & Simulation.Statistics.StatsPer.Tick) == Simulation.Statistics.StatsPer.Tick) {
                    CurrentPer = Simulation.Statistics.StatsPer.Tick;
        }
        return;

      default: 
        return;
    }
  }

  public abstract void CalculateLayout();

  public abstract void Refresh();

  protected virtual bool TryConnect() {

        StatSourceInfo = Simulation.Statistics.GetDescription(_statSourceType, _statId);
    // 
    if (WarnThreshold == 0 && ErrorThreshold == 0) {
      WarnThreshold = StatSourceInfo.WarnThreshold;
      ErrorThreshold = StatSourceInfo.ErrorThreshold;
    }

    if (gameObject.activeInHierarchy == false) {
      return false;
    }

    if (_fusionStats == null) {
      _fusionStats = GetComponentInParent<FusionStats>();
    }

    // Any data connection requires a runner for the statistics source.
    var runner = _fusionStats?.Runner;

    var statistics = runner?.Simulation?.Stats;
    //Stats.StatSourceInfo info;


    switch (_statSourceType) {
      case Simulation.Statistics.StatSourceTypes.Simulation: {
                    _statsBuffer = statistics?.GetStatBuffer((Simulation.Statistics.SimStats)_statId);
          break;
        }
      case Simulation.Statistics.StatSourceTypes.NetworkObject: {
          if (_statId >= Simulation.Statistics.OBJ_STAT_TYPE_COUNT) {
                        StatId = 0;
          }
          if (_fusionStats.Object == null) {
                        _statsBuffer = null;
            break;
          }

                    _statsBuffer = statistics?.GetObjectBuffer(_fusionStats.Object.Id, (Simulation.Statistics.ObjStats)_statId, true);
          break;
        }
      case Simulation.Statistics.StatSourceTypes.NetConnection: {

          //StatSourceInfo = Stats.GetDescription((Stats.NetStats)_statId);
          if (runner == null) {
                        _statsBuffer = null;
            break;
          }
                    _statsBuffer = statistics?.GetStatBuffer((Simulation.Statistics.NetStats)_statId, runner);

          break;
        }
      default: {
                    _statsBuffer = null;
          break;
        }
    }
    if (BackImage) {
      BackImage.color = BackColor;
    }

    // Update the labels, regardless if a connection can be made.
    if (LabelTitle) {
      CheckIfValidIncurrentMode(runner);
      ApplyTitleText();
    }


    CurrentPer = StatSourceInfo.PerDefault;

    return (_statsBuffer != null);
  }

  protected void ApplyTitleText() {
    var info = StatSourceInfo;

    // stat info is invalid
    if (info.LongName == null) {
      return;
    }

    if (info.InvalidReason != null) {
      LabelTitle.text = info.InvalidReason;
      BackImage.gameObject.SetActive(false);
      LabelTitle.color = _fusionStats.FontColor * new Color(1, 1, 1, 0.2f);
    } else {
      var titleRT = LabelTitle.rectTransform;
      if (titleRT.rect.width < 100) {
        LabelTitle.text = info.ShortName ?? info.LongName;
      } else {
        LabelTitle.text = info.LongName;
      }
      BackImage.gameObject.SetActive(true);
    }
  }

  protected void CheckIfValidIncurrentMode(NetworkRunner runner) {
    if (runner == false) {
      return;
    }

    //var info = StatSourceInfo;
    var flags = StatSourceInfo.Flags;

    if ((flags & Simulation.Statistics.StatFlags.ValidForBuildType) == 0) {
            StatSourceInfo.InvalidReason = "DEBUG DLL ONLY";
      return;
    }

    var obj = _statSourceType == Simulation.Statistics.StatSourceTypes.NetworkObject ? _fusionStats?.Object : null;

    if (obj) {
      bool nonStateAuthOnly = (flags & Simulation.Statistics.StatFlags.ValidOnStateAuthority) == 0;
      if (nonStateAuthOnly && obj.HasStateAuthority) {
        StatSourceInfo.InvalidReason = "NON STATE AUTH ONLY";
        return;
      }
    }

    if (runner) {
      bool clientOnly = (flags & Simulation.Statistics.StatFlags.ValidOnServer) == 0;
      if (clientOnly && runner.IsClient == false) {
        StatSourceInfo.InvalidReason = "CLIENT ONLY";
        return;
      }
      bool ecOnly = (flags & Simulation.Statistics.StatFlags.ValidWithDeltaSnapshot) == 0;
      if (ecOnly && runner.Config.Simulation.ReplicationMode == SimulationConfig.StateReplicationModes.DeltaSnapshots) {
        StatSourceInfo.InvalidReason = "EC MODE ONLY";
        return;
      }
    }
  }
}
