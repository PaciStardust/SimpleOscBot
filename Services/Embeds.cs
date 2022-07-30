using Discord;

namespace SimpleOscBot.Services
{
    /// <summary>
    /// Class to automatically create simple embeds
    /// </summary>
    public static class Embeds
    {
        public static EmbedFieldBuilder CreateField(string name, string value, bool inline = false) => new EmbedFieldBuilder()
            .WithValue(value)
            .WithName(name)
            .WithIsInline(inline);

        /// <summary>
        /// Automatically logs the error
        /// </summary>
        public static Embed Error(string type, string message, string trace = "unspecified location", bool log = true)
        {
            if (log)
                Logger.Error(type, message, "Embeds", trace);

            return new EmbedBuilder()
            {
                Title = "An error has occured",
                Description = "Please contact bot development",
                Color = Color.Red

            }.AddField(type, message).Build();
        }
        /// <summary>
        /// Automatically logs the error
        /// </summary>
        public static Embed Error(Exception error, bool log = true) => Error(error.GetType().FullName, error.Message, error.StackTrace, log);

        public static Embed Info(string title, string message, Color? color = null) => new EmbedBuilder()
        {
            Title = title,
            Description = message,
            Color = color ?? Color.LightGrey
        }.Build();

        public static Embed InvalidInput() => Info("Invalid input", "The input you have given is invalid");
        public static Embed NoResults() => Info("No Results", "Result list is empty, if you think this is a mistake, please contact bot dev");
        public static Embed InvalidGuild() => Info("Invalid Guild", "Command was not run within guild");
        public static Embed DbFailure() => Error("Database Error", "The database encountered an error during a requrest", log: false);
    }
}
