import { useEffect, useState } from "react";
import { Form, Modal, Input, Select } from "antd";
import { materialsNew } from "../api/api";
import toast from "react-hot-toast";

export default function ModalCreateFile({ isModalOpen, folder, closeModal, onMaterialUpdated }) {
    const [form] = Form.useForm();
    const [selectedFile, setSelectedFile] = useState(null);

    useEffect(() => {
        if (isModalOpen) {
            form.setFieldsValue({
                name: "Имя файла",
                description: "Описание",
                status: folder.code -2 
            });
        }
    }, [isModalOpen, form]);


    const handleFileSelect = (event) => {
        const file = event.target.files[0];
        console.log("Выбран файл:", file);
        
        if (file) {
            setSelectedFile(file);
        } else {
            setSelectedFile(null);
        }
    };


    const handleOk = async () => {
    try {
        const values = await form.validateFields();

        console.log(values)

        const fileName = selectedFile.name;
        const fileExtension = fileName.split('.').pop();
        
        const formData = new FormData();
            formData.append('name', values.name);
            formData.append('description', values.description);
            formData.append('status', values.status);
            formData.append('folderId', folder.id);
            formData.append('path', folder.path + "/" + folder.name);
            formData.append('type', "."+fileExtension);
            formData.append('file', selectedFile);
        
         console.log("Отправляем файл:", {
                name: values.name,
                description: values.description,
                status: values.status,
                folderId: folder.id,
                path: folder.name,
                fileName: selectedFile.name,
                fileSize: selectedFile.size,
                fileType: selectedFile.type
            });

        const response = await materialsNew(formData);

        if (response.error === "") {
            toast.success(`Файл ${values.name} создан`);
            closeModal();
            form.resetFields();
            if (onMaterialUpdated) onMaterialUpdated();
        } else {
            toast.error(response.error);
        }
    } catch (error) {
        console.log(error);
        toast.error(error.message || "Произошла ошибка");
    }
};

    const handleCancel = () => {
        closeModal();
        form.resetFields();
    };

    return (
        <Modal
            title={"Создать файл"}
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
                        label="Имя"
                        rules={[{ required: true, message: 'Введите имя файла' }]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        name="description"
                        label="Описание"
                        rules={[{ required: false }]}
                    >
                        <Input.TextArea  
                            rows={3} 
                        />
                    </Form.Item>

                    <Form.Item
                        name="status"
                        label="Доступ других пользователей"
                        rules={[{ required: true, message: 'Выберите уровень доступа' }]}
                    >
                        <Select
                            options={[
                            folder.code === 2 && { value: 0, label: 'Режим просмотра' },
                            { value: 1, label: 'Скрытый (другие не увидят этот файл)' }
                                ].filter(option => option !== false)}
/>
                    </Form.Item>

                    <Form.Item 
                        name="file"
                        label="Выбор файла"
                        rules={[{required: true, message: 'Выберите файл для загрузки'}]}
                    >
                            <Input type='file' onChange={handleFileSelect}/>

                    </Form.Item>
                </Form>
        </Modal>
    );
}