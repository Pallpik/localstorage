import { useEffect, useState } from "react"
import { getUserInfo } from "../api/api"
import { Button, Card } from "antd";
import toast from "react-hot-toast"
export default function UserProfilePage( {toMain, updateAuth} ){

    const [data, setData] = useState(null)
    const [loading, setLoading] = useState(true)

    useEffect (() =>{
        getUserInfo()
        .then(data => {
            console.log('Полученные данные:', data);
            setData(data);
            setLoading(false);
        }

        ).catch(error => {
                console.error('Ошибка загрузки папок:', error);
                setLoading(false);
            });


    }, []);

    const handleExit = () => {
        toast.success("Вы вышли из системы!"); 
        localStorage.removeItem('token'); 
        updateAuth()
        toMain()
    }

    if (loading) return <div className="loading">Загрузка данных...</div>;
    if (!data) return <div className="empty-state"> Нет доступных данных</div>
    return (<Card title="Профиль">
        <p>{data.name}</p>
        <p>{data.login}</p>
        <p>{data.role}</p>
        <Button type='primary' onClick={handleExit}>Выйти</Button>
    </Card>
    ); 
}