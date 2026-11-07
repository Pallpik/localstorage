import toast from "react-hot-toast";

const BASE = `https://localhost:7219`

console.log(BASE)

export async function getFileBlob(id) {
    const token = localStorage.getItem('token');
    let res = null;
    try{
        res = await fetch(`${BASE}/materials/download/${id}`,{
        headers: {
            "Authorization": `Bearer ${token}`
        }
    }); 
    }
    catch(error){
        
        console.log(res)
        console.log(error)
    }

    if (res.status === 401) {
        localStorage.removeItem('token');
        
        alert("Ваша сессия устарела, зайдите в профиль")
        return {status: 401}
        
    }

    if (!res.ok) {
        console.log("Error: ", await res.json());
        const text = await res.text().catch(() => null);
        //throw new Error(text || `HTTP ${res.status}`);
  }
  
  return await res.blob();
}



async function request(path) {
    const token = localStorage.getItem('token');
    let res = null;
    try{
        res = await fetch(`${BASE}${path}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
        });

    }
    catch(error){

        toast.error("Произошла ошибка, повторите позже")
        console.log(error)

    }
    

    if (res.status === 401) {
        localStorage.removeItem('token');
        
        alert("Ваша сессия устарела, зайдите в профиль")
        return {status: 401}
        
    }


    if (!res.ok) {
        console.log("Error: ", await res.json());
        const text = await res.text().catch(() => null);
        //throw new Error(text || `HTTP ${res.status}`);
    }
    
    return await res.json();
}




async function postData(url ="", data = {}){
  const token = localStorage.getItem('token');
    let res = null
    try{
       res = await fetch(`${BASE}${url}`, {
      method: "POST",
      headers: {
        "Content-Type" : "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify(data),
    });
    }
    catch(error){
        
        toast.error("Произошла ошибка, повторите позже")
        console.log(error)
    }

    if (res.status === 401) {
        
        localStorage.removeItem('token');
        alert("Ваша сессия устарела, зайдите в профиль")
        return {status: 401}
        
    }

    const responseData = await res.json().catch(() => null);
    
    if (!res.ok) {
        console.log('Error response data:', responseData);
        console.log('Error ID:', responseData?.id);
        console.log('Error message:', responseData?.message);
    }
    
    return responseData;
}

async function putData(url = "", data = {}){
  const token = localStorage.getItem('token');
    let res = null 
    try{
      res = await fetch(`${BASE}${url}`, {
      method: "PUT",
      headers: {
        "Content-Type" : "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify(data),
    });
    }
    catch(error){
        
        toast.error("Произошла ошибка, повторите позже")
        console.log(error)
    }

    if (res.status === 401) {
        
        localStorage.removeItem('token');
        alert("Ваша сессия устарела, зайдите в профиль")
        return {status: 401}
        
    }

    const responseData = await res.json().catch(() => null);
    
    if (!res.ok) {
        console.log('Error response data:', responseData);
        console.log('Error ID:', responseData?.id);
        console.log('Error message:', responseData?.message);
    }
    
    return responseData;

}

async function deleteData(url = ""){
  const token = localStorage.getItem('token');
    let res = null
    try{
      res = await fetch(`${BASE}${url}`,{
      method: "DELETE",
      headers: {
        "Authorization": `Bearer ${token}`
      },
    }
    );
    }
    catch(error){
        
        toast.error("Произошла ошибка, повторите позже")
        console.log(error)
    }

    if (res.status === 401) {
        
        localStorage.removeItem('token');
        alert("Ваша сессия устарела, зайдите в профиль")
        return {status: 401}
        
    }

    const responseData = await res.json().catch(() => null);
    
    if (!res.ok) {
        console.log('Error response data:', responseData);
        console.log('Error ID:', responseData?.id);
        console.log('Error message:', responseData?.message);
    }
    
    return responseData;
}


export function fetchAllFolders(){
    return request('/folders/get')
}

export function fetchUserFolders(){
    return request(`/folders/my`)
}

export function fetchFolder(Id){
    return request(`/folders/${Id}`)
}

export function updateFolder(data){
    return putData("/folders/update", data);
}

export function createFolder(data){
    return postData("/folders/create", data);
}


export function deleteFolder(folderId){
    return deleteData(`/folders/delete/${folderId}`);
}


export function fetchMaterialInfo(Id){
    return request(`/materials/${Id}`)
}

export async function materialsNew(formData) {
    const token = localStorage.getItem('token');
    let res = null
    try{
        res = await fetch(`${BASE}/materials/new`, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${token}`
        },
        body: formData,
        });
    }
    catch(error){
        console.log(error)
        toast.error("Произошла ошибка, повторите позже")
    }

    const responseData = await res.json().catch(() => null);
    
    if (!res.ok) {
        console.log('Error response data:', responseData);
        console.log('Error ID:', responseData?.id);
        console.log('Error message:', responseData?.message);
    }
    
    return responseData;
}

export function materialsUpdate(data){

    /*{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "path": "string",
  "description": "string",
  "oldName": "string",
  "type": "string",
  "status": 0
}*/ 
    return putData(`/materials/update`, data)
}

export function materialsRedirect(data){
    return putData(`/materials/redirect`, data)
}

export function materialsDelete(Id){
    return deleteData(`/materials/delete/${Id}`)
}

// export function getFile(Id){
//     return request(`/materials/download/${Id}`)
// }


export function getUserInfo(){
    return request(`/mypage/profile`)
}

export function mypageDelete(){
    return deleteData(`/mypage/delete`)
} 

export function mypageEditData(data){
    return putData(`/mypage/edit/data`, data)
}

export function mypageEditInfo(data){
    return putData(`/mypage/edit/info`, data)
}




export function register(registerData){
    return postData(`/Auth/register`, registerData)
}

export async function login(loginData) {
    try {
        const response = await fetch(`${BASE}/Auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(loginData),
        });
        
        if (!response.ok) {
            const errorText = await response.text();
            return {success: false, error: errorText}
        }

        const token = await response.text();
        
        localStorage.setItem('token', token);
      
        
        return { success: true, token: token };
    } catch (error) {
        console.error("Login error:", error);
        toast.error("Произошла ошибка, повторите позже")
    }

}