
public static class ARAppUITexts
{
    public const string VersionString = "Preview Version 1";
    public const string DateDisplayCulture = "en-GB";

    // in case one cannot connect to the server
    public const string ButtonTryAgain = "Try again!";
    public const string ButtonRefresh = "Refresh";

    // button to join a channel
    public const string ButtonJoin = "Start";

    public const string ButtonMoreInformation = "More Information";

    // text that might be displayed on startup
    public const string PreMenuCannotConnect = "Cannot connect to ARquatic server. Please connect to the Internet or try again later.";
    public const string PreMenuNotCompatibleApple = "Your device is not compatible with ARKit.Unfortunately ARquatic will not run on your device.";
    public const string PreMenuNotCompatibleAndroid = "Your device is not compatible with ARCore. Unfortunately ARquatic will not run on your device.";
    public const string PreMenuAppOutOfDate = "Your ARquatic app is out of date. Please update your app in order to experience ARquatic.";
    public const string PreMenuARNeedsInstall = "In order to run this app ARCore needs to be installed from the Play Store. Your device should display a prompt for that.";

    public const string MainMenuAvailableChannelsLabel = "Available Channels";
    public const string MainMenuInformationLabel = "Information";

    public const string MainMenuChannelInformationError = "Could not refresh channel information...";

    public const string MainMenuStatusOnline = "online";
    public const string MainMenuStatusOffline = "offline";

    // status messages displayed in the AR experience
    public const string ARStatusWaitingForTracking = "Move your device slowly to establish tracking ...";
    public const string ARStatusTapOnCube = "... tap on the cube to place the AR!";

    public const string ARStatusInsufficientLight = "Insufficient light in your room to establish stable tracking.";
    public const string ARStatusInsufficientFeatures = "Insufficient optical features in your room to establish stable tracking.";

    static public void PopulateAbout(AboutPopulator x)
    {
        x.Headline("About");
        x.Paragraph("This can be a short text about the app, the project and everything else ... Let's keep it short!");
        x.LinkButton("CodeKlavier Homepage", "https://codeklavier.space/");

        x.ParagraphDivider();

        x.Headline("Credits");
        x.Paragraph("Best Piano Lady\nSweet Prince\nWhiskey the Cat\nGerman Moustache Dude");

        x.ParagraphDivider();

        x.Headline("Juan Appreciation");
        x.Paragraph("Juan is a nice dude. He should have initially been part of the project but could not unfortunately because he is now Corporate Juan.");
    }
}
