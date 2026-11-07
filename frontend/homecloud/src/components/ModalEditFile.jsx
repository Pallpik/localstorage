import { useEffect } from "react";
import { Form, Modal, Input, Select } from "antd";
import { materialsUpdate } from "../api/api";
import toast from "react-hot-toast";

export default function ModalEditFile({ isModalOpen, material, folder, closeModal, onMaterialUpdated }) {
    const [form] = Form.useForm();

    useEffect(() => {
        if (isModalOpen && material) {
            form.setFieldsValue({
                name: material.name,
                description: material.description,
                status: material.code - 2
            });
        }
    }, [isModalOpen, material, form]);

    const handleOk = async () => {
        if (!material) {
            toast.error("Материал не выбран");
            return;
        }
        
        try {
            const values = await form.validateFields();
            const data = {
                id: material.id,
                name: values.name,
                path: material.path,
                description: values.description,
                oldName: material.name,
                type: material.type,
                status: values.status
            };
            
            const response = await materialsUpdate(data);

            if (response.error === "") {
                toast.success(`Файл ${material.name} обновлён`);
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
            title={material ? `Изменить файл ${material.name}` : "Изменить файл"}
            open={isModalOpen}
            onOk={handleOk}
            onCancel={handleCancel}
            okText="Сохранить"
            cancelText="Отмена"
        >
            {material ? (
                <Form
                    form={form}
                    layout="vertical"
                    initialValues={{
                        name: material.name,
                        description: material.description,
                        status: material.code - 2
                    }}
                >
                    <Form.Item
                        name="name"
                        label="Новое имя"
                        rules={[{ required: true, message: 'Введите имя файла' }]}
                    >
                        <Input placeholder={material.name} />
                    </Form.Item>

                    <Form.Item
                        name="description"
                        label="Описание"
                        rules={[{ required: false }]}
                    >
                        <Input.TextArea 
                            placeholder={material.description || "Введите описание"} 
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
                                ...(folder.code === 2 ? [{ value: 0, label: 'Режим просмотра' }] : []),
                                { value: 1, label: 'Скрытый (другие не увидят эту папку)' }
                            ]}
                        />
                    </Form.Item>
                </Form>
            ) : (
                <div>Загрузка...</div>
            )}
        </Modal>
    );
}