using _ProjectBeta.Scripts.ScriptableObjects.Player;

public class Stats 
{
    public float MaxHealth { get => _maxHp; private set => _maxHp = value; }
    public float BaseMovementSpeed { get => _baseMovementSpeed; private set => _baseMovementSpeed = value; }
    public float BaseDefense { get => _baseDefense; private set => _baseDefense = value; }



    private float _maxHp;
    private float _baseMovementSpeed;
    private float _baseDefense;



    public Stats(PlayerData data)
    {
        _maxHp = data.MaxHealth;
        _baseMovementSpeed = data.BaseMovementSpeed;
        _baseDefense = data.BaseDefense;
    }

    public Stats(float maxHealth, float baseDefense, float baseMovementSpeed = 0)
    {
        _maxHp = MaxHealth;
        this._baseMovementSpeed = baseMovementSpeed;
        this._baseDefense = baseDefense;
    }
}
