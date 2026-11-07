import { 
  FileWordTwoTone, AudioTwoTone, FilePptTwoTone, FilePdfTwoTone, 
  FileImageTwoTone, VideoCameraTwoTone, FileTextTwoTone, 
  FileExcelTwoTone, FileZipTwoTone, FileTwoTone
} from '@ant-design/icons';

  export const getMaterialIcon = (type) => {
    const typeLower = (type || '').toLowerCase();
    if (typeLower.includes('pdf')) return <FilePdfTwoTone />;
    if (typeLower.includes('image') || typeLower.includes('jpg') || typeLower.includes('png')) return <FileImageTwoTone />;
    if (typeLower.includes('video')) return <VideoCameraTwoTone />;
    if (typeLower.includes('audio')) return <AudioTwoTone />;
    if (typeLower.includes('word') || typeLower.includes('doc')) return <FileWordTwoTone />;
    if (typeLower.includes('txt')) return <FileTextTwoTone />; 
    if (typeLower.includes('excel') || typeLower.includes('xls')) return <FileExcelTwoTone />;
    if (typeLower.includes('ppt') || typeLower.includes('pptx')) return <FilePptTwoTone />;
    if (typeLower.includes('zip') || typeLower.includes('rar')) return <FileZipTwoTone />;
    return <FileTwoTone />;
  };