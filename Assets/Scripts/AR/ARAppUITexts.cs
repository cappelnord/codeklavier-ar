
public static class ARAppUITexts
{
    public const string VersionString = "Version 1.1";
    public const string DateDisplayCulture = "en-GB";

    // in case one cannot connect to the server
    public const string ButtonTryAgain = "Try again!";
    public const string ButtonRefresh = "Refresh";

    // button to join a channel
    public const string ButtonJoin = "Start";

    // button to get more info
    public const string ButtonMoreInformation = "More Information";

    // text that might be displayed on startup
    public const string PreMenuCannotConnect = "Cannot connect to ARquatic server. Please connect to the Internet or try again later.";
    public const string PreMenuNotCompatibleApple = "Your device is not compatible with ARKit. Unfortunately ARquatic will not run on your device.";
    public const string PreMenuNotCompatibleAndroid = "Your device is not compatible with ARCore. Unfortunately ARquatic will not run on your device.";
    public const string PreMenuAppOutOfDate = "Your ARquatic app is out of date. Please update your app in order to experience ARquatic.";
    public const string PreMenuARNeedsInstall = "In order to run this app ARCore needs to be installed from the Play Store. Your device should display a prompt for that.";

    public const string MainMenuAvailableChannelsLabel = "Available Channels";
    public const string MainMenuInformationLabel = "Information";

    public const string MainMenuChannelInformationError = "Could not refresh channel information ...";

    public const string MainMenuStatusOnline = "online";
    public const string MainMenuStatusOffline = "offline";

    // status messages displayed in the AR experience
    public const string ARStatusWaitingForTracking = "Move your device slowly to establish tracking!";
    public const string ARStatusTapOnCube = "Tap to place the AR!";

    public const string ARStatusInsufficientLight = "Insufficient light in your environment to establish stable tracking.";
    public const string ARStatusInsufficientFeatures = "Insufficient optical features in your view to establish stable tracking.";

    static public void PopulateAbout(AboutPopulator x)
    {
        x.Headline("About");
        x.Paragraph("The ARquatic app creates the Augmented Reality (AR) visuals during an 'ARquatic Live' experience. At showtime, simply follow the app prompts to watch the AR underwater world unfold before your eyes.");
        x.Paragraph("The 'ARquatic Live' experience is a CodeKlavier project. A key aspect of this experience is that the AR visuals are built on L-systems that are coded, (yes coded!), with the help of the CodeKlavier, by the pianist in real-time during the show. 'ARquatic Live' is an audio-visual experience with the same pianist and a collaborating laptop musician, performing music live.");
        x.Paragraph("Keep reading or visit our website for more info about how 'ARquatic Live' works or ask the host!");
        x.LinkButton("ARquatic Live", "https://codeklavier.space/arquatic");

        x.ParagraphDivider();

        x.Headline("Credits");
        x.Paragraph("Anne Veinberg - piano\nFelipe Ignacio Noriega - laptop audio\nPatrick Borgeat - visuals");

        x.ParagraphDivider();

        x.Headline("Artistic Concept");
        x.Paragraph("Codeklavier’s ‘ARquatic Live’ is an audio and Augmented Reality experience where the live music provides the DNA of the underwater world unfolding before your eyes. Full of speculative, fantasy structures artificially built from L-system rules, ‘ARquatic Live’ superimposes unusual aquatic inspired objects in everyday environments. Whether you attend a live performance or tune in at home for a streamed experience, the juxtaposition from the surroundings with the AR objects will stimulate one’s imagination by highlighting the contrasting beauty of the natural and the unnatural world.");

        x.ParagraphDivider();

        x.Headline("Motivation");
        x.Paragraph("Our inspiration for working with L-systems comes primarily from its roots in nature and organic growth which we would like to bring to the musical and programming paradigms. Whilst L-systems are already widely used in generative art and music, we are interested in exploring how this simple model can be used by the piano coder to express complex structures and how its rules will shape the pianistic improvisation. Furthermore, Lindenmayer’s work was done here at the University of Utrecht in 1968 and building upon it through artistic exploration, makes the Netherlands a particularly special location for the presentation of this project.");

        x.ParagraphDivider();

        x.Headline("Privacy");
        x.ParagraphDivider();
        x.LinkButton("ARquatic Privacy Policy", "https://codeklavier.space/privacy");
#if UNITY_ANDROID
        x.ParagraphDivider();
        x.Paragraph("This application runs on Google Play Services for AR (ARCore), which is provided by Google LLC and governed by the Google Privacy Policy.");
        x.LinkButton("Google Privacy Policy", "https://policies.google.com/privacy");
#endif
    }
}
