import { Button, Card, Form, Input } from "antd"
import FormItem from "antd/es/form/FormItem";
import { useEffect, useState } from "react"
import { login } from "../api/api";
import toast from "react-hot-toast";


export default function LoginPage( { toRegister, toMain, updateAuth } ){


    const [form] = Form.useForm();

    const handleLogin = async () => {
            let ltoast = null
            try {
            const values = await form.validateFields();
            
            console.log("Данные для логина:", values);
            ltoast =  toast.loading("Загрузка...");
            const response = await login(values)
            if(response.success === true){
                toast.dismiss(ltoast)
                toast.success('Вы вошли!')
                updateAuth(); 
                setTimeout(() => {
                    toMain();
                }, 100);
            }
            else{
                toast.dismiss(ltoast)
                toast.error("Неправильный логин или пароль!")
            }      
            

        } catch (error) {
                toast.dismiss(ltoast)
            console.error("Ошибка валидации:", error);
        }


    }


    return (
        <div style={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            padding: '20px'     // небольшие отступы по краям на узких экранах
        }}>
        <Card title="Вход" style = {{width: '60%', boxShadow:'0 4px 12px rgba(0, 0, 0, 0.15)'}} >
        <Form form = {form} layout="vertical">
            
            <Form.Item 
                name="login"
                label="Логин"
                rules={[{ required: true, message: 'Введите логин' }]}
                >

                <Input placeholder="Введите логин" />
            </Form.Item>
            
            <Form.Item
                name="password"
                label="Пароль"
                rules={[{ required: true, message: 'Введите пароль' }]}
                
            >
                <Input.Password placeholder="Введите пароль" />
            </Form.Item>
            <Form.Item>
                <Button type="primary" onClick={() => handleLogin()}>Войти</Button>
            </Form.Item>    
            <Form.Item style={{ textAlign: 'right', marginBottom: '8px' }}>
                        <span>
                            <a onClick={toRegister} style={{ cursor: 'pointer' }}>
                                Нет профиля? Регистрация
                            </a>
                        </span>
            </Form.Item>


    </Form></Card></div>)
}