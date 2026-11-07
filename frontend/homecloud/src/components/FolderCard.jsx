import { Form, Modal, Button, Row, Col, Input, Select } from 'antd'; 
import { FolderOutlined } from '@ant-design/icons';
import { useState } from 'react';
import { deleteFolder, updateFolder } from '../api/api';
import toast from "react-hot-toast"

function FolderCard({folder, onOpen, onFolderDelete}){
    const [isHovered, setIsHovered] = useState(false);

    const [isModalOpen, setIsModalOpen] = useState(false);

    const [form] = Form.useForm();

    const handleEdit = (e) => {
        setIsHovered(false)
        e.stopPropagation();
        form.setFieldsValue({
            name: folder.name,
            access: folder.code - 2
        });
        setIsModalOpen(true);
    };

    const handleOk = async () => {
        
        const values = await form.validateFields();
        try {
            let response = await updateFolder({
                Id: folder.id,
                newName: values.name,
                Path: folder.path,
                Status: values.access
            });
            console.log(`Response PUT folder ${response.error}`);
            if (response.error === "" || response.error == "" || response.error === null){
                toast.success("Папка успешно обновлена");
                folder.name = values.name;
                folder.code = values.access;
                setIsModalOpen(false);
                form.resetFields();
            }
            else{
                toast.error(`Ошибка: ${response.error}`);
            }

        } catch (error) {
            toast.error(values);
            console.error('Validation failed:', error);
        }
    };

    const handleCancel = () => {
        setIsModalOpen(false);
        form.resetFields();
    };

    async function handleDelete(e){
        setIsHovered(false); 
        if (window.confirm('Вы уверены, что хотите удалить эту папку?')) {
        try{
            let response = await deleteFolder(folder.id);
            console.log(`Response DELETE folder ${response.error}`);
            if (response.error === "" || response.error == "" || response.error === null){
                toast.success("Папка успешно удалена");
                await onFolderDelete();
                form.resetFields();
            }
            else{
                toast.error(`Ошибка: ${response.error}`);
            }
        }
        catch (error){
            toast.error(error);
            console.error('Validation failed:', error);
        }} 
    else {
      toast('Действие отменено', { icon: 'ℹ️' });
    }
    }

    const functions = (f) =>{
        return (
            <div onClick={(e) => e.stopPropagation()}>
                {(f.code === 2 || f.code === 3) && (
                    <>
                    <Button 
                        onClick={handleEdit}
                        style={{
                            padding: '4px 12px',
                            fontSize: '16px',
                            cursor: 'pointer',
                            marginRight: '8px'
                        }}
                    >
                        ✏️ 
                    </Button>
                    <Button 
                        onClick={handleDelete}
                        style={{
                            padding: '4px 12px',
                            fontSize: '16px',
                            cursor: 'pointer'
                        }}
                    >
                        ✖️
                    </Button>
                    </>
                )}
            </div>
        );
    }   

    return (
        <div style={{ width: '100%' }}>
            <Row 
                style={{ 
                    width: '100%', 
                    display: 'flex', 
                    textAlign: 'left',
                    color: 'black',
                    cursor: 'pointer',
                    padding: '8px 0px 8px 0px',
                    backgroundColor: isHovered ? 'rgb(165, 205, 237)' : 'transparent',
                    transition: 'background-color 0.3s'
                }}
                onMouseEnter={() => (setIsHovered(true))}
                onMouseLeave={() => (setIsHovered(false) )}
                onClick={() => (isHovered && !isModalOpen && onOpen(folder))}
            > 
                <Col span={9}>
                    <FolderOutlined/>
                    <span style={{padding:'10px'}}>{folder.name}</span>
                </Col>
                <Col span={5}>
                    <span>{folder.author}</span>
                </Col>
                <Col span={5}>
                    <span>{folder.materials.length}</span>
                </Col>
                <Col span={5}>
                    {functions(folder)}
                    <Modal
                        title={"Изменить папку " + folder.name}
                        open={isModalOpen}
                        onOk={handleOk}
                        onCancel={handleCancel}  
                        okText="Сохранить"
                        cancelText="Отмена"
                    >     
                         <Form
                    form={form}
                    layout="vertical"
                >
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
                </Col>
            </Row>
        </div>
    );
}

export default FolderCard;