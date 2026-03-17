using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;

public class FelonyStopMenu : ModUIMenu
{
    private MenuPool MenuPool;
    private UIMenu Menu;
    private IPoliceRespondable Player;
    private UIMenuItem Comply;
    private UIMenuItem Resist;
    private bool ChoiceMade;

    public FelonyStopMenu(MenuPool menuPool, IPoliceRespondable policeRespondable)
    {
        MenuPool = menuPool;
        Player = policeRespondable;
    }

    public void Setup()
    {
        Menu = new UIMenu("Felony Stop", "~r~Pull Over~s~ - Officers have identified you from a BOLO/APB");
        Menu.SetBannerType(EntryPoint.LSRedColor);
        Menu.OnMenuClose += OnMenuClose;
        MenuPool.Add(Menu);
    }

    public override void Hide()
    {
        Menu.Visible = false;
    }

    public override void Show()
    {
        if (!Menu.Visible)
        {
            Create();
            ChoiceMade = false;
            Menu.Visible = true;
        }
    }

    public override void Toggle()
    {
        if (!Menu.Visible)
        {
            Show();
        }
        else
        {
            Menu.Visible = false;
        }
    }

    private void OnMenuClose(UIMenu sender)
    {
        if (!ChoiceMade && Player.IsInFelonyStop)
        {
            Player.OnFelonyStopResist();
        }
    }

    private void Create()
    {
        Menu.Clear();

        Comply = new UIMenuItem("Comply", "~g~Pull over and submit to arrest.~s~ Officers will place you in handcuffs.");
        Comply.RightBadge = UIMenuItem.BadgeStyle.Lock;
        Comply.Activated += (sender, selectedItem) =>
        {
            ChoiceMade = true;
            Player.OnFelonyStopComply();
            Menu.Visible = false;
        };
        Menu.AddItem(Comply);

        Resist = new UIMenuItem("Resist", "~r~Attempt to evade the officers.~s~ You are now a wanted suspect - expect a pursuit.");
        Resist.RightBadge = UIMenuItem.BadgeStyle.Alert;
        Resist.Activated += (sender, selectedItem) =>
        {
            ChoiceMade = true;
            Player.OnFelonyStopResist();
            Menu.Visible = false;
        };
        Menu.AddItem(Resist);
    }
}
