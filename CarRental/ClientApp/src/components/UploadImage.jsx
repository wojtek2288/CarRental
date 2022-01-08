import React, { Fragment, useState } from 'react';
import { Input, Button, Progress, Alert } from 'reactstrap';
import axios from 'axios';

const UploadImage = () => {
    const [selectedFile, setSelectedFile] = useState(null);
    const [hideAlert, setHideAlert] = useState(true);
    const [alertText, setAlertText] = useState("");
    const [progress, setProgress] = useState(0);
    const [fileName, setFileName] = useState("brick_wall2-disp-51223ca91d2-e8da-4834-8f84-42605b1a5066.png");
    const fileSelectedHandler = (event) => {
        setSelectedFile(event.target.files[0]);
        setProgress(0);
        setHideAlert(true);
    }

    const fileUploadHandler = async () => {
        const fd = new FormData();
        fd.append('image', selectedFile, selectedFile.name);
        try
        {
            const res = await axios.post('/File/save', fd,
            {
                onUploadProgress: progressEvent => {
                    setProgress(Math.round(progressEvent.loaded / progressEvent.total * 100));
                }
            });
            console.log(res);
        }
        catch (err)
        {
            console.log(err);
            if (err.response.status === 400) {
                setAlertText(err.response.data);
                setHideAlert(false);
            }
        }
    }

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
        <Fragment>
            <Alert color='danger' hidden={hideAlert}>{alertText}</Alert>
            <p>Supported extensions: .jpg, .jpeg, .png</p>
            <p>Max file size: 2MB</p>
            <Input type='file' onChange={fileSelectedHandler} accept="image/*"/>
            <Progress value={progress}/>
            <Button color='primary' onClick={fileUploadHandler}>Upload</Button>
            <Button color='primary' onClick={() => fileDownloadHandler(fileName)}>Download File</Button>
        </Fragment>
        );
}

export default UploadImage;