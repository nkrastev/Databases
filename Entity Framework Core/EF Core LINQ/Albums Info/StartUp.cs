namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);

            //Test your solutions here
            Console.Write("Enter Producer ID: ");
            int producerId = int.Parse(Console.ReadLine());

            Console.WriteLine(ExportAlbumsInfo(context, producerId));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var allAlbums = context
                .Albums
                .Where(x=>x.ProducerId==producerId)
                .Select(x=> new
                {
                    AlbumName=x.Name,
                    AlbumDate=x.ReleaseDate,
                    AlbumPrice=x.Price,
                    ProducerName=x.Producer.Name,
                    Songs=x.Songs.Select(s=> new
                    {
                        SongName=s.Name,
                        SongPrice=s.Price,
                        SongWriter=s.Writer.Name
                    }).OrderByDescending(o=>o.SongName).ThenBy(o=>o.SongWriter).ToList()
                })
                .OrderByDescending(x=>x.AlbumPrice)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var item in allAlbums)
            {
                sb.AppendLine($"-AlbumName: {item.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {item.AlbumDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {item.ProducerName}");
                sb.AppendLine($"-Songs:");
                int counter = 1;
                foreach (var song in item.Songs)
                {
                    sb.AppendLine($"---#{counter}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.SongPrice:f2}");
                    sb.AppendLine($"---Writer: {song.SongWriter}");
                    counter++;
                }
                sb.AppendLine($"-AlbumPrice: {item.AlbumPrice:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
