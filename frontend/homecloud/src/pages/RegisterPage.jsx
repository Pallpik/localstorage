import { Card, Form, Input, Button } from "antd"
import toast from "react-hot-toast";
import { register } from "../api/api";

export default function RegisterPage({ toLogin }) { 

    const [form] = Form.useForm();

    const handleReg = async () => {
        try {
            const values = await form.validateFields();
            
            if (values.password !== values.SecPassword) {
                toast.error("Пароли не совпадают!")
                return;
            }
            
            console.log("Данные для регистрации:", values);
            const response = await register(values)
            
            console.log(response.error)

            if (response.error === ''){
                toast.success("Вы зарегистрировались! Войдите в профиль");
                toLogin();
            }
            else{
                toast.error(response.error)
            }            

        } catch (error) {
            toast.error("Ошибка валидации:", error);
        }
    }

    return (
        <div style={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            padding: '20px'
        }}>
            <Card title="Регистрация" size="small"style={{  width: '60%', boxShadow: '0 4px 12px rgba(0, 0, 0, 0.15)' }}>
                <Form form={form} size="medium" layout="vertical">
                    <Form.Item  
                        name="name"
                        label="Имя"
                        rules={[{ required: true, message: 'Введите имя' }]}
                        style={{ marginBottom: '8px' }} 
                    >
                        <Input placeholder="Введите ваше имя" />
                    </Form.Item>
                    
                    <Form.Item 
                        name="login"
                        label="Логин"
                        rules={[{ required: true, message: 'Введите логин' }]}
                        style={{ marginBottom: '8px'}} 
                    >
                        <Input placeholder="Придумайте логин" />
                    </Form.Item>
                    
                    <Form.Item
                        name="password"
                        label="Пароль"
                        rules={[{ required: true, message: 'Введите пароль' }]}
                        style={{ marginBottom: '8px' }} 
                    >
                        <Input.Password placeholder="Введите пароль" />
                    </Form.Item>
                    
                    <Form.Item
                        name="SecPassword"
                        label="Повтор пароля"
                        rules={[
                            { required: true, message: 'Введите пароль ещё раз' },
                            ({ getFieldValue }) => ({
                                validator(_, value) {
                                    if (!value || getFieldValue('password') === value) {
                                        return Promise.resolve();
                                    }
                                    return Promise.reject(new Error('Пароли не совпадают'));
                                },
                            }),
                        ]}
                        style={{ marginBottom: '8px' }} 
                    >
                        <Input.Password placeholder="Подтвердите пароль" />
                    </Form.Item>
                    
                    <Form.Item 
                        style={{ marginBottom: '8px' }} 
                    >
                        <Button type="primary" onClick={handleReg} >
                            Зарегистрироваться
                        </Button>
                    </Form.Item>

                    <Form.Item style={{ textAlign: 'right', marginBottom: '8px' }}>
                        <span>
                            <a onClick={toLogin} style={{ cursor: 'pointer' }}>
                                Уже есть профиль? Войти
                            </a>
                        </span>
                    </Form.Item>
                </Form>
            </Card>
        </div>
    )
}