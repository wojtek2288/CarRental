import React, { Fragment, useState } from 'react';
import { Input, Button, Progress, Alert } from 'reactstrap';
import axios from 'axios';

const UploadImage = () => {
    const [selectedFile, setSelectedFile] = useState(null);
    const [hideAlert, setHideAlert] = useState(true);
    const [alertText, setAlertText] = useState("");
    const [progress, setProgress] = useState(0);
    
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
            const res = await axios.post('/savefile/image', fd,
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

    return (
        <Fragment>
            <Alert color='danger' hidden={hideAlert}>{alertText}</Alert>
            <p>Supported extensions: .jpg, .jpeg, .png</p>
            <p>Max file size: 2MB</p>
            <Input type='file' onChange={fileSelectedHandler} accept="image/*"/>
            <button type='button' onClick={fileUploadHandler}>Wyślij</button>
            <Progress value={progress}/>
        </Fragment>
        );
}

export default UploadImage;