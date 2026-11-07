import { useEffect, useState } from "react";
import { Breadcrumb, Row, Table, Space, Button, Col, Input } from "antd";
import getStringFromCode from "../components/AccessCodesDecoder";
import { getMaterialIcon } from "../components/IconGiver";
import { getFileBlob, materialsDelete, fetchFolder } from "../api/api"; // Добавьте fetchFolder в импорт
import { toast } from 'react-hot-toast';
import ModalEditFile from "../components/ModalEditFile";
import ModalCreateFile from "../components/ModalCreateFile";

export default function FolderPage({ folder, onBack, onOpenMaterial }) {
  const [materials, setMaterials] = useState([]);
  const [query, setQuery] = useState("");
  const [isEditModalOpen, setEditModalOpen] = useState(false);
  const [isCreateModalOpen, setCreateModalOpen] = useState(false);
  const [selectedMaterial, setSelectedMaterial] = useState(null);
  const [loading, setLoading] = useState(false); // Добавьте состояние загрузки

  // Загрузка материалов при монтировании и изменении folder
  useEffect(() => {
    loadMaterials();
  }, [folder.id]); // Зависимость от folder.id

  const loadMaterials = async () => {
    setLoading(true);
    try {
      // Если folder приходит с материалами, используем их
      if (folder.materials && folder.materials.length > 0) {
        setMaterials(folder.materials);
      } else {
        // Иначе загружаем заново
        const updatedFolder = await fetchFolder(folder.id);
        setMaterials(updatedFolder.materials || []);
      }
    } catch (error) {
      console.error('Ошибка загрузки материалов:', error);
      toast.error('Ошибка загрузки материалов');
    } finally {
      setLoading(false);
    }
  };

  const handleDownload = async (record) => {
    console.log('Загрузить:', record);
    try {
      const blob = await getFileBlob(record.id);
      const url = URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = record.name;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      URL.revokeObjectURL(url);
    } catch (error) {
      console.error('Ошибка скачивания:', error);
      toast.error('Ошибка скачивания файла');
    }
  };

  const handleEdit = (record) => {
    console.log('Редактировать:', record);
    setSelectedMaterial(record);
    setEditModalOpen(true);
  };

  // ИСПРАВЛЕННАЯ функция updateList
  const updateList = async () => {
    try {
      setLoading(true);
      const updatedFolder = await fetchFolder(folder.id);
      setMaterials(updatedFolder.materials || []);
      toast.success('Список обновлен');
    } catch (error) {
      console.error('Ошибка обновления списка:', error);
      toast.error('Ошибка обновления списка');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (record) => {
    if (window.confirm('Вы уверены, что хотите удалить этот файл?')) {
      try {
        const response = await materialsDelete(record.id);
        if (!response.status) {
          if (response.error === "") {
            toast.success('Запись удалена');
            await updateList(); // Обновляем список после удаления
          } else {
            toast.error(response.error);
          }
        }
      } catch (error) {
        console.log(error);
        toast.error('Произошла ошибка, повторите позже');
      }
    } else {
      toast('Действие отменено', { icon: 'ℹ️' });
    }
  };

  const handleCreate = () => {
    console.log('Создание');
    setCreateModalOpen(true);
  };

  const handleView = (record) => {
    console.log('Просмотр:', record);
    onOpenMaterial(record);
  };

  function convertDataToSource(m) {
    return {
      key: String(m.id),
      id: String(m.id),
      name: m.name,
      type: m.type,
      description: m.description,
      author: m.author,
      path: m.path,
      code: m.code
    };
  }
  
  // ИСПРАВЛЕННАЯ фильтрация
  const filteredMaterials = materials.filter(material => 
    material.name.toLowerCase().includes(query.toLowerCase()) && material.code !== 1
  );

  const columns = [
    {
      title: 'Имя',
      width: '25%',
      dataIndex: 'name',
      key: 'name',
      ellipsis: true,
      render: (text, record) => (
        <span style={{ 
          display: 'flex',
          alignItems: 'center',
          gap: '8px'
        }}>
          {getMaterialIcon(record.type)} 
          <span style={{ wordBreak: 'break-word' }}>{text}</span>
        </span>
      )
    },
    {
      title: 'Тип',
      width: '25%',
      dataIndex: 'type',
      key: 'type',
      ellipsis: true,
    },
    {
      title: 'Доступ',
      width: '25%',
      dataIndex: 'code', 
      key: 'code',
      ellipsis: true,
      render: (_, record) => getStringFromCode(record.code)
    },
    {
      title: 'Действия',
      width: '25%',
      key: 'actions',
      render: (_, record) => (
        <div style={{ 
          display: 'flex',
          flexDirection: 'column', 
          gap: '8px',
        }}>
          <Row>
            {record.code !== 1 && (
              <>
                <Col>
                  <Button 
                    onClick={() => handleView(record)}
                    size="small"
                    style={{
                      padding: '4px 12px',
                      fontSize: '16px',
                      cursor: 'pointer'
                    }}
                  >
                    👀
                  </Button>
                </Col>
                <Col>
                  <Button 
                    onClick={() => handleDownload(record)}
                    size="small"
                    style={{
                      padding: '4px 12px',
                      fontSize: '16px',
                      cursor: 'pointer'
                    }}
                  >
                    📥
                  </Button>
                </Col>
              </>
            )}
          </Row>
          <Row>
            {(record.code === 2 || record.code === 3) && (
              <>
                <Col>
                  <Button
                    onClick={() => handleEdit(record)}
                    size="small"
                    style={{
                      padding: '4px 12px',
                      fontSize: '16px',
                      cursor: 'pointer'
                    }}
                  >
                    ✏️
                  </Button>
                </Col>
                <Col>
                  <Button 
                    size="small"
                    onClick={() => handleDelete(record)}
                    style={{
                      padding: '4px 12px',
                      fontSize: '16px',
                      cursor: 'pointer'
                    }}
                  >
                    ✖️
                  </Button>
                </Col>
              </>
            )}
          </Row>
        </div>
      ),
    }
  ];

  function changeSearch(e) {
    setQuery(e.target.value);
  }

  return (
    <div style={{ width: "100%" }}>
      <Row align="middle" style={{ flexWrap: 'wrap', gap: '12px', marginBottom: '16px' }}>
        <Col span={12}>
          <Breadcrumb 
            style={{ fontSize: "16px", padding: '8px 16px' }} 
            items={[
              { title: <a onClick={onBack}>Папки</a> },
              { title: folder.name }
            ]}
          />
        </Col>
        <Col span={4}>
          <Input 
            type={"search"} 
            name="q" 
            placeholder="Поиск..." 
            onChange={changeSearch}
            value={query}
            allowClear
          />
        </Col>
        <Col span={4}>
          <Button 
            onClick={onBack}
            style={{
              padding: '8px 16px',
              fontSize: '14px',
              cursor: 'pointer'
            }}
          >
            Назад
          </Button>
        </Col>
      </Row>
      
      <Table 
        columns={columns} 
        dataSource={filteredMaterials.map(convertDataToSource)}
        rowKey="key"
        size="middle"
        loading={loading}
        locale={{ emptyText: query ? "Ничего не найдено" : "Нет материалов" }}
      />
      
       {(folder.code === 2 || folder.code === 3) && <Button style={{margin:'8px'}} onClick={handleCreate}>+ Создать файл</Button> }
      
      <ModalEditFile 
        isModalOpen={isEditModalOpen} 
        material={selectedMaterial} 
        folder={folder} 
        closeModal={() => {
          setEditModalOpen(false);
          setSelectedMaterial(null);
        }} 
        onMaterialUpdated={() => updateList()}
      />
      
      <ModalCreateFile 
        isModalOpen={isCreateModalOpen} 
        folder={folder} 
        closeModal={() => setCreateModalOpen(false)} 
        onMaterialUpdated={() => updateList()}
      />
    </div>
  );
}