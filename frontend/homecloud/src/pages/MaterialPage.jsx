import { Breadcrumb, Col, Divider, Row } from "antd";
import { getMaterialIcon } from "../components/IconGiver";
import Preview from "../components/PreviewMachine";


export default function MaterialPage({ material, folderName, onBackToFolder, onBackToFolders}){






    return ( <div>

            <Breadcrumb 
              style={{ fontSize: "16px", padding: '8px 16px' }} 
              items={[
                { title: <a onClick={onBackToFolders}>Папки</a> },
                { title: <a onClick={onBackToFolder}>{folderName} </a> },
                { title: <span>{getMaterialIcon(material.type)} {material.name}{material.type} </span>}
              ]}
            />  
            <Row>
            <Col span={4}>
            <h3>Автор:</h3>
            <h3>{material.author}</h3>
            <Divider type="horizontal"></Divider>
            <h3>Описание:</h3> 
            <span>{material.description}</span>
            </Col>
            <Col style={{display:'flex', alignItems:'left', alignContent:'left'}} span={20}>
                <Preview material={material} />
            </Col>
            </Row>

    </div>);

}