using MediaCatalog.Model.DTO;

namespace MediaCatalog.Model.Interfaces
{
    public interface IMediaInfoProvider
    {
        MediaFileDTO GetMediaFileInfo(string fileName, TV_ProgramDTO parent);
        bool IsMediaFile(string fileName);
    }
}
