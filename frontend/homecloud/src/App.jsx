import React, { useEffect, useState } from 'react';
import HomePage from './pages/HomePage.jsx';
import FoldersPage from './pages/FoldersPage.jsx';
import FolderPage from './pages/FolderPage.jsx';
import { Button, Menu} from 'antd'
import UserProfilePage from './pages/UserProfilePage.jsx';
import toast, {Toaster } from "react-hot-toast"
import MaterialPage from './pages/MaterialPage.jsx';
import RegisterPage from './pages/RegisterPage.jsx';
import LoginPage from './pages/LoginPage.jsx';

const getNavItems = () => {
  const items = [
    { label: 'HomeCloud', key: 'home', whiteSpace: 'normal', wordWrap: 'break-word'},
    { key: 'divider-0', type: 'divider', style: {margin:'100% 0 0 0'} },
    { label: 'Все папки', key: 'folders', whiteSpace: 'normal', wordWrap: 'break-word' },
    { label: 'Мои папки', key: 'userFolders', whiteSpace: 'normal', wordWrap: 'break-word' },
  ];
  
  const isNotAuthenticated = !(localStorage.getItem('token') || localStorage.getItem('token') === 'null');
  if (isNotAuthenticated) {
    items.push(
      { label: 'Логин', key: 'login', whiteSpace: 'normal', wordWrap: 'break-word' },
      { label: 'Регистрация', key: 'register', whiteSpace: 'normal', wordWrap: 'break-word' }
    );
  }
  else{
    items.push(
      { label: 'Профиль', key: 'profile', whiteSpace: 'normal', wordWrap: 'break-word' }
    );   
  }
  
  return items;
};

function App() {

  const [view, setView] = useState('login');
  const [selectedFolder, setSelectedFolder] = useState(null);
  const [selectedMaterial, setSelectedMaterial] = useState(null);
  const [isAuthd, setAuthd] = useState(false)
  const [navItems, setNavItems] = useState(getNavItems)

  const updateAuthState = () => {
    const token = localStorage.getItem('token');    
    const isAuthenticated = token && token !== 'null';
    setAuthd(isAuthenticated);
    if (!isAuthd){
      setView('login')
    }
    setNavItems(getNavItems());
    return isAuthenticated;
  };

  useEffect(() => {
    updateAuthState();
  }, []);

  function openFolder(f){
    setSelectedFolder(f);
    setView('folder');
    window.scrollTo(0,0);
  }

  function openMaterial(m){
    setSelectedMaterial(m);
    setView('material');
    window.scrollTo(0,0);
  }

  function onClick(e){
    console.log('click', e);
    setView(e.key);
  }

  return (
    <div style={{width: "100%", display: "flex", flexDirection:"row",  height: "100vh"}}>
      <Toaster />
      <Menu style = {{height: "100%", alignItems: 'center'}} title= "HomeCloud" theme="dark" onClick ={onClick} selectedKeys={view} mode="vertical" items={navItems} />
      <main style = {{width: "99%", padding:'0% 0% 0% 1%'}}>
        {view === 'home' ? <HomePage /> : <>
        { isAuthd ? ( 
          <>
        {view === 'folders' && <FoldersPage onOpenFolder={openFolder} />}
        {view === 'folder' && <FolderPage folder={selectedFolder} onBack={() => setView('folders')} onOpenMaterial={openMaterial} />}
        {view === 'userFolders' && <FoldersPage onOpenFolder={openFolder} isUserFolders={true}/>}
        {view === 'profile' && <UserProfilePage toMain={() => setView('home')} updateAuth={updateAuthState}/>}
        {view === 'material' && <MaterialPage material={selectedMaterial} folderName={selectedFolder.name} onBackToFolder={() => setView('folder')} onBackToFolders={() => setView('folders')} />}
         </>
        ) : (
          <>
            {view != 'login' && view != 'register' && <div style={{padding:'20px'}}>
            <h2>Вы ещё не вошли в систему!</h2> 
            <Button style={{margin:'20px 0px 0px 0px'}}onClick={() => setView('login')}>Войти</Button> </div>
            } 
            
          </>
          )}
          
        </>
        
        }
        {view === 'login' && <LoginPage toRegister={() => setView('register')} toMain={() => setView('home')} updateAuth={updateAuthState}/>}
        {view === 'register' && <RegisterPage toLogin={() => setView('login')}/>}
      </main>
    </div>
  );
}

export default App;