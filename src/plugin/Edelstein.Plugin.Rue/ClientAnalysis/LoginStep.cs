namespace Edelstein.Plugin.Rue.ClientAnalysis;

/// <summary>
/// Client-side CLogin->m_nLoginStep values.
///
/// NOTE: These are DIFFERENT from the server-side <see cref="Protocol.Gameplay.Login.LoginState"/> enum!
/// Server LoginState: CheckPassword=0, SelectGender=1, SelectWorld=2, SelectCharacter=3, Connecting=4
/// Client LoginStep:  Title=0, SelectWorld=1, SelectCharacter=2, NewCharacter=3, etc.
///
/// The client transitions steps via CLogin::ChangeStep(-1) which computes (m_nLoginStep + 1) % 6.
/// Each step has an associated fade animation; m_tStepChanging is non-zero during transitions.
/// </summary>
public enum LoginStep : byte
{
    /// <summary>Title screen (initial state). CLogin::ChangeStepImmediate creates CLoginGradeWnd.</summary>
    Title = 0,

    /// <summary>World selection. CUIWorldSelect is created, WorldRequest packet sent.</summary>
    SelectWorld = 1,

    /// <summary>Character selection. CUICharSelect + CUIAvatar created after SelectWorldResult.</summary>
    SelectCharacter = 2,

    /// <summary>New character race selection. CUINewCharRaceSelect created.</summary>
    NewCharacter = 3,

    /// <summary>New character avatar/name selection. CUINewCharAvatarSelect created.</summary>
    NewCharacterName = 4,

    /// <summary>View All Characters (VAC) mode. CUIAvatarVAC created.</summary>
    VAC = 5
}
