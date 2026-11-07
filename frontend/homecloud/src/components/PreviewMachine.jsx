import { useEffect, useState } from "react";
import { getFileBlob } from "../api/api.js";
import DocxPreviewer from "./DocxPreview.jsx";



export default function PreviewMachine({ material }) {
  const [previewUrl, setPreviewUrl] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [mimeType, setMimeType] = useState(null);
  const [textContent, setTextContent] = useState(null);
  const [wordContent, setWordContent] = useState(null);

  useEffect(() => {

    console.log(material?.id);

    if (!material?.id) return;

    let objectUrl = null;
    let isMounted = true;

    const loadPreview = async () => {
      setLoading(true);
      setError(null);
      setPreviewUrl(null);
      setTextContent(null);

      try {

        const blob = await getFileBlob(material.id);
        
        setMimeType(blob.type);

        if (blob.type.startsWith('application/msword') || blob.type.startsWith('application/vnd.openxmlformats-officedocument.wordprocessingml.document')){
          if (isMounted) setWordContent(blob);
        }

        if (blob.type.startsWith('text/') || blob.type === 'application/json') {
          const text = await blob.text();
          if (isMounted) setTextContent(text);
        } 
        
        objectUrl = URL.createObjectURL(blob);
        setPreviewUrl(objectUrl);

          
        console.log('Blob size:', blob.size, 'Blob type:', blob.type);
  console.log('Detected mimeType:', mimeType);
console.log('Preview URL:', objectUrl);

      } catch (err) {
        if (isMounted) setError(err.message || 'Failed to load preview');
      } finally {
        if (isMounted) setLoading(false);
      }
    };

    loadPreview();

    console.log("objectUrl")
    return () => {
      isMounted = false;
      if (objectUrl) URL.revokeObjectURL(objectUrl);
    };
  }, [material.id, material.name, material.type]);

  if (loading) return <div style={{ padding: '1rem' }}>Загрузка превью...</div>;
  if (error) return <div style={{ padding: '1rem', color: 'red' }}>Ошибка: {error}</div>;
  if (!previewUrl && !textContent && !error && !loading) return <div>Превью не доступно :P</div>;

  if(wordContent !== null){
    return (
     <DocxPreviewer fileBlob={wordContent}></DocxPreviewer>
    );
  }

  if (textContent !== null) {
    return (
      <pre style={{ fontSize:'12px', tabSize: '6', lineHeight:'15px',textAlign:'left', whiteSpace: 'pre', width: '95%', border: '3px solid #4a4949', maxHeight: `80%`, overflow: 'auto', background: '#f5f5f5' }}>
        {textContent}
      </pre>
    );
  }

  if (mimeType?.startsWith('image/')) {
    return <img src={previewUrl} alt={material.name} style={{ width: '95%', border: '3px solid #4a4949', maxHeight: '80vh' }} />;
  }

  if (mimeType?.startsWith('video/')) {
    return <video controls src={previewUrl} style={{ width: '95%', border: '3px solid #4a4949', maxHeight: '80vh' }} />;
  }

  if (mimeType?.startsWith('audio/')) {
    return <audio controls src={previewUrl} style={{ width: '95%', border: '3px solid #4a4949'}} />;
  }

  if (mimeType === 'application/pdf') {
    return <iframe src={previewUrl} title={material.name} style={{ width: '95%', height: '80vh', border: '3px solid #4a4949' }} />;
  }

  return (
    <div style={{width:'100%'}}>
      <p>Превью не поддерживает данный тип файла.</p>
      <a href={previewUrl} download={material.name}>Скачать {material.name}</a>
    </div>
  );
}