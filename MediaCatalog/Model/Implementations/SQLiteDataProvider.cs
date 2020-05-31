using MediaCatalog.Model.DTO;
using MediaCatalog.Model.Interfaces;
using System.Collections.Generic;

namespace MediaCatalog.Model.Implementations
{
    public class SQLiteDataProvider : IProgramsProvider
    {
        MediaCatalogDBContext DBContext;
        List<TV_ProgramDTO> AllPrograms;
        List<MediaFileDTO> AllMedia;

        public SQLiteDataProvider()
        {
            DBContext = new MediaCatalogDBContext();
            AllPrograms = new List<TV_ProgramDTO>(DBContext.TVPrograms);
            AllMedia = new List<MediaFileDTO>(DBContext.MediaFiles);
        }

        public IEnumerable<TV_ProgramDTO> GetPrograms()
        {
            return AllPrograms;
        }

        public async void AddProgramAsync(TV_ProgramDTO Program)
        {
            await DBContext.TVPrograms.AddAsync(Program);
            CommitChangesAsync();
        }

        public async void AddVideoFileAsync(MediaFileDTO video)
        {
            await DBContext.MediaFiles.AddAsync(video);
            CommitChangesAsync();
        }

        public void RemoveVideoFile(MediaFileDTO video)
        {
            DBContext.MediaFiles.Remove(video);
            CommitChangesAsync();
        }

        public void Remove(TV_ProgramDTO Program)
        {
            DBContext.Remove(Program);
            CommitChangesAsync();
        }

        public async void CommitChangesAsync()
        {
            await DBContext.SaveChangesAsync();
        }
    }
}
