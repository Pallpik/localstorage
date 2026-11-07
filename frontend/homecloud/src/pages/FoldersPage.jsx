import { createFolder, fetchAllFolders, fetchUserFolders } from '../api/api';
import { useEffect, useState } from 'react';
import { Button, Col, Divider, Form, Input, List, Modal, Row, Select } from 'antd';
import FolderCard from "../components/FolderCard"
import { toast } from 'react-hot-toast';

export default function FoldersPage({ onOpenFolder, isUserFolders }) {
    const [folders, setFolders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchLine, setLine] = useState("");
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [form] = Form.useForm();

    // Функция загрузки папок
    const loadFolders = async () => {
        setLoading(true);
        try {
            let data;
            if (isUserFolders === true) {
                console.log("Загрузка пользовательских папок");
                data = await fetchUserFolders();
            } else {
                console.log("Загрузка всех папок");
                data = await fetchAllFolders();
            }
            console.log('Полученные папки:', data);
            setFolders(data || []);
        } catch (error) {
            console.error('Ошибка загрузки папок:', error);
            toast.error('Ошибка загрузки папок');
            setFolders([]);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadFolders();
    }, [isUserFolders]); // Зависимость от isUserFolders

    function handleCreate() {
        setIsModalOpen(true);
    }

    const handleOk = async () => {
        try {
            const values = await form.validateFields();
            const loadingToast = toast.loading("Создание папки...");
            
            const response = await createFolder({
                Name: values.name,
                Path: "",
                Status: values.access
            });
            
            console.log(`Response POST folder:`, response);
            
            if (response.error === "" || response.error === null || !response.error) {
                toast.dismiss(loadingToast);
                toast.success("Папка успешно создана");
                
                // Перезагружаем список папок
                await loadFolders();
                
                setIsModalOpen(false);
                form.resetFields();
            } else {
                toast.dismiss(loadingToast);
                toast.error(`${response.error}`);
            }
        } catch (error) {
            toast.dismiss(loadingToast);
            toast.error(`Ошибка: ${error.message}`);
            console.log('Validation failed:', error);
        }
    };

    const handleCancel = () => {
        setIsModalOpen(false);
        form.resetFields();
    };

    // Функция обновления списка (для передачи в FolderCard)
    const updateFoldersList = async () => {
        await loadFolders();
    };

    if (loading) return <div className="loading">Загрузка папок...</div>;

    if (!folders || folders.length === 0) {
        return (
            <div style={{ width: "100%" }}>
                <Row align="middle" style={{ flexWrap: 'wrap', display: 'flex', textAlign: 'left', gap: '12px', marginBottom: '20px' }}>
                    <Col span={12}> 
                        <p style={{ fontSize: '16px', color: 'black' }}>Список папок</p> 
                    </Col>
                    <Col span={8}> 
                        <Input 
                            type="search" 
                            name="q" 
                            placeholder="Поиск..." 
                            onChange={changeSearch}
                            value={searchLine}
                            allowClear
                        /> 
                    </Col>
                </Row>
                <div style={{ padding: '10px' }}>Папок ещё нет, время их создать!</div>
                <Button style={{ margin: '8px 0px 0px 0px' }} onClick={handleCreate}>+ Создать папку</Button>
                
                <Modal
                    title="Создать папку"
                    open={isModalOpen}
                    onOk={handleOk}
                    onCancel={handleCancel}
                    okText="Создать"
                    cancelText="Отмена"
                >
                    <Form form={form} layout="vertical">
                        <Form.Item
                            name="name"
                            label="Название папки"
                            rules={[{ required: true, message: 'Введите название папки' }]}
                        >
                            <Input placeholder="Введите название" />
                        </Form.Item>
                        
                        <Form.Item
                            name="access"
                            label="Доступ других пользователей"
                            rules={[{ required: true, message: 'Выберите уровень доступа для других пользователей' }]}
                        >
                            <Select
                                defaultValue={0}
                                options={[
                                    { value: 0, label: 'Режим просмотра' },
                                    { value: 1, label: 'Скрытый (другие не увидят эту папку)' }
                                ]}
                            />
                        </Form.Item>
                    </Form>
                </Modal>
            </div>
        );
    }

    function searchFolders(query, folder) {
        if (!query || query === "") return true;
        return (folder.name?.toLowerCase().includes(query.toLowerCase()) || 
                folder.author?.toLowerCase().includes(query.toLowerCase()));
    }

    function changeSearch(e) {
        setLine(e.target.value);
    }

    const filteredFolders = !folders.status && folders.filter(folder => searchFolders(searchLine, folder));

    return (
        <div style={{ width: "100%" }}>
            <Row align="middle" style={{ flexWrap: 'wrap', display: 'flex', textAlign: 'left', gap: '12px' }}>
                <Col span={12}> 
                    <p style={{ fontSize: '16px', color: 'black' }}>Список папок</p> 
                </Col>
                <Col span={8}> 
                    <Input 
                        type="search" 
                        name="q" 
                        placeholder="Поиск..." 
                        onChange={changeSearch}
                        value={searchLine}
                        allowClear
                    /> 
                </Col>
            </Row>
            
            {filteredFolders.length > 0 && (
                <>
                    <Row style={{ padding: '2% 0 0 0', width: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center', textAlign: 'left' }}>
                        <Col span={8}>Имя</Col>
                        <Col span={1}><Divider type="vertical" /></Col>
                        <Col span={4}>Автор</Col>
                        <Col span={1}><Divider type="vertical" /></Col>
                        <Col span={4}>Файлов</Col>
                        <Col span={1}><Divider type="vertical" /></Col>
                        <Col span={4}>Инструменты</Col>
                        <Col span={1}><Divider type="vertical"/></Col>
                    </Row>

                    {filteredFolders.map(f => (
                        <FolderCard 
                            key={f.id} 
                            folder={f} 
                            onOpen={onOpenFolder} 
                            onFolderDelete={updateFoldersList}
                        />
                    ))}
                </>
            )}
            
            {filteredFolders.length === 0 && searchLine && (
                <div style={{ padding: '20px', textAlign: 'center' }}>
                    По запросу "{searchLine}" ничего не найдено
                </div>
            )}
            
            <Modal
                title="Создать папку"
                open={isModalOpen}
                onOk={handleOk}
                onCancel={handleCancel}
                okText="Создать"
                cancelText="Отмена"
            >
                <Form form={form} layout="vertical">
                    <Form.Item
                        name="name"
                        label="Название папки"
                        rules={[{ required: true, message: 'Введите название папки' }]}
                    >
                        <Input placeholder="Введите название" />
                    </Form.Item>
                    
                    <Form.Item
                        name="access"
                        label="Доступ других пользователей"
                        rules={[{ required: true, message: 'Выберите уровень доступа для других пользователей' }]}
                    >
                        <Select
                            options={[
                                { value: 0, label: 'Режим просмотра' },
                                { value: 1, label: 'Скрытый (другие не увидят эту папку)' }
                            ]}
                        />
                    </Form.Item>
                </Form>
            </Modal>
            
            <Button style={{ margin: '8px 0px 0px 0px' }} onClick={handleCreate}>+ Создать папку</Button>
        </div>
    );
}