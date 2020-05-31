using System.Collections.Generic;
using MediaCatalog.Model.DTO;

namespace MediaCatalog.Model.Interfaces
{
    public interface IProgramsProvider
    {
        IEnumerable<TV_ProgramDTO> GetPrograms();
        void AddProgramAsync(TV_ProgramDTO Program);
        void AddVideoFileAsync(MediaFileDTO video);
        void RemoveVideoFile(MediaFileDTO video);
        void Remove(TV_ProgramDTO Program);
        void CommitChangesAsync();
    }
}
