import React from "react";
import { useEffect } from "react";
import { Button } from "reactstrap";

const ReturnData = (props) =>{
    useEffect(() => {
        props.refresh(true);
    }, []);

    const imageName = props.hist.imagename; 
    const documentName = props.hist.documentname; 

    console.log(props.hist);
    console.log(imageName);
    console.log(documentName);

    const fileDownloadHandler = (fileName) => {
        if (fileName !== null && fileName !== '' && fileName !== undefined) {
            fetch('/File/download/' + fileName, {
                responseType: 'blob'
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
    }

    return (
        <div hidden={props.hidden}>
            <b>Note:</b> <p>{props.hist.note}</p>
            
            <b>Car Picture</b><br/>
            <Button
                disabled={imageName === null || imageName === '' || imageName === undefined}
                onClick={() => fileDownloadHandler(imageName)}
                color="primary" size="sm" >
                💾 Download
            </Button>
            <br/>
            <br/>
            <b>Document</b><br/>
            <Button
                disabled={documentName === null || documentName === '' || documentName === undefined}
                onClick={() => fileDownloadHandler(documentName)}
                color="secondary" size="sm">
                💾 Download
            </Button>
        
        </div>
    );
};

export default ReturnData;