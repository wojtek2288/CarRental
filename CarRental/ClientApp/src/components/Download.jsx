import React from "react";
import { useState } from "react";
import { Button } from "reactstrap";

const ReturnData = (props) =>{

    // const imageName = props.hist.imagename; 
    // const documentName = props.hist.documentname; 

    const [imageName, setImageName] = useState("brick_wall2-disp-51223ca91d2-e8da-4834-8f84-42605b1a5066.png");
    const [documentName, setDocumentName] = useState("brick_wall2-disp-51223ca91d2-e8da-4834-8f84-42605b1a5066.png");
    
    const fileDownloadHandler = (fileName) => {

        fetch('/File/download/' + fileName,{
                responseType: 'blob',
            })
            .then(response => {
                if (response.ok) {
                    return response.blob();
                }
            })
            .then(blob => {
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', fileName); 
                document.body.appendChild(link);
                link.click();
            });
    }

    return (
        <div hidden={props.hidden}>
            <b>Note:</b> <p>{props.hist.note}</p>
            
            <b>Car Picture</b><br/>
            <Button onClick={()=>fileDownloadHandler(imageName)} 
            color="primary" size="sm" >💾 Download</Button>
            <br/>
            <br/>
            <b>Document</b><br/>
            <Button onClick={() => fileDownloadHandler(documentName)}
            color="secondary" size="sm">💾 Download</Button>
        
        </div>
    );
};

export default ReturnData;