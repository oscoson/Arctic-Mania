public interface IFreezable
{
    void Freeze();  // Freezing logic goes here
    void UnFreeze();
    bool IsFrozen();  // Outputs whether the entity is frozen or not
    void CheckFreeze();
}
