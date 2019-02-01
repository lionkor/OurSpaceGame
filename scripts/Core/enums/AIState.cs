public enum AiState
{
    /// <summary>
    /// Roaming around without any goal
    /// </summary>
    Idle,
    /// <summary>
    /// Roaming around looking for something to kill
    /// </summary>
    Searching,
    /// <summary>
    /// Did just see the player and is trying to get 
    /// line of sight again
    /// </summary>
    CombatSearching,
    /// <summary>
    /// Seeking the player and shooting at them
    /// </summary>
    Combat,
    /// <summary>
    /// Backing off of target
    /// </summary>
    Backoff,
    /// <summary>
    /// Travelling to a certain point
    /// </summary>
    Travelling
}
