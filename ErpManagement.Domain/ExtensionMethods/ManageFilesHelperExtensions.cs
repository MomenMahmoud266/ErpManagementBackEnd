namespace ErpManagement.Domain.Extensions;

public static class ManageFilesHelperExtensions 
{
    public static GetFileNameAndExtension UploadFile(IFormFile file, string path)
    {
        string fileName = Guid.NewGuid() + "_" + file.FileName;
        string finalFilePath = Path.Combine(Directory.GetCurrentDirectory() + path, fileName);

        using (var Stream = new FileStream(finalFilePath, FileMode.Create))
        {
            file.CopyTo(Stream);
        };
        return new GetFileNameAndExtension
        {
            FileName = fileName,
            FileExtension = Path.GetExtension(file.FileName)
        };
    }

    public static List<GetFileNameAndExtension> UploadFiles(IFormFileCollection files, string path)
    {
        List<GetFileNameAndExtension> list = [];
        foreach (var file in files)
        {
            string fileName = Guid.NewGuid() + "_" + file.FileName;
            string finalFilePath = Path.Combine(Directory.GetCurrentDirectory() + path, fileName);
            using (var Stream = new FileStream(finalFilePath, FileMode.Create))
            {
                file.CopyTo(Stream);
            };
            list.Add(new GetFileNameAndExtension
            {
                FileName = fileName,
                FileExtension = Path.GetExtension(file.FileName)
            });
        }
        return list;
    }

    public static void RemoveFile(string file)
    {
        file = Directory.GetCurrentDirectory() + file;
            if (File.Exists(file))
                File.Delete(file);
    }

    public static void RemoveFiles(List<string> files)
    {
        foreach (string file in files)
        {
            string fullPath = Directory.GetCurrentDirectory() + file;

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }

    //public static List<GetFileNameAndExtension> UploadFiles(IFormFileCollection files, string path)
    //{
    //    List<GetFileNameAndExtension> list = new();
    //    foreach (var file in files)
    //    {
    //        string fileName = Guid.NewGuid() + "_" + file.FileName;
    //        string finalFilePath = Path.Combine(Directory.GetCurrentDirectory() + path, fileName);
    //        using (var Stream = new FileStream(finalFilePath, FileMode.Create))
    //        {
    //            file.CopyTo(Stream);
    //        };
    //        list.Add(new GetFileNameAndExtension
    //        {
    //            FileName = fileName,
    //            FileExtension = Path.GetExtension(file.FileName)
    //        });
    //    }
    //    return list;
    //}

}
