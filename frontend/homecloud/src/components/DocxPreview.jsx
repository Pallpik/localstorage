import { useEffect, useRef } from 'react';
import { renderAsync } from 'docx-preview';

const DocxPreviewer = ({ fileBlob }) => {
  const containerRef = useRef(null);

  useEffect(() => {
    if (containerRef.current && fileBlob) {
      containerRef.current.innerHTML = '';
      const docxOptions = {
               
                ignoreWidth: true,        // Игнорируем ширину страницы
                ignoreHeight: true,       // Игноририуем высоту страницы
                useBase64URL: true,       // Для корректного отображения изображений
                debug: false,
            };

      renderAsync(fileBlob, containerRef.current, null, docxOptions);
    }
  }, [fileBlob]);

  return <div ref={containerRef} style={{ border: '3px solid #4a4949', width: '95%', maxHeight: '80vh', overflow: 'auto' }} />;
};

export default DocxPreviewer;