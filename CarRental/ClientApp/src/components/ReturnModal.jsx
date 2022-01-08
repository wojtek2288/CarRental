import React, { Fragment, useState } from 'react';
import { Button, FormGroup, Card, CardBody, CardTitle, Input, Form, Label, Alert, Spinner} from 'reactstrap';
import Modal from 'react-modal';
import axios from 'axios';

const ReturnForm = (props) => {
    let carstates = ['Good','Average','Bad'];
    const [formData, setFormData] = useState({
        odometer: 0,
        description: '',
        carstate: 'Good'
    });
    const [selectedFile, setSelectedFile] = useState(null);    
    const [hideAlert, setHideAlert] = useState(true);
    const [alertText, setAlertText] = useState("");
    const [progress, setProgress] = useState(0);

    const onChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    }
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


    const close = () =>{
        props.closeModal();
    }

    return (
        <Fragment>
            <Modal
                isOpen={props.modalIsOpen}
                onRequestClose={props.closeModal}
                contentLabel="Example Modal"
                ariaHideApp={false}
                style={{
                    overlay: {
                        backgroundColor: 'rgba(255,255,255,0)',
                        top: '4vh',
                        left: '1rem',
                        right: '1rem',
                        bottom: '4vh',
                    }
                }}
                transparent
            >
                <Button className="closeBtn" onClick={close} close />
                <Form>
                    <FormGroup>
                        <Card className='margin-bottom'>
                            <CardBody>
                                <CardTitle tag="h5">
                                    Check Returned Car
                                </CardTitle>

                                <Label for="odometer">
                                    Odometer Value
                                </Label>
                                <Input id="odometer"
                                    name="odometer"
                                    placeholder="odometer placeholder"
                                    type="number"
                                    value={formData.odometer}
                                    onChange={(e) => onChange(e)}
                                    className='margin-bottom'
                                    required
                                />
                                
                                <Label for="carstate">
                                    Overall State
                                </Label>
                                <Input id="carstate"
                                    name="carstate"
                                    placeholder="carstate placeholder"
                                    type="select"
                                    value={formData.carstate}
                                    onChange={(e) => onChange(e)}
                                    className='margin-bottom'
                                    required>
                                {carstates.map((state, key) => <option key={key}>{state}</option>)}
                                </Input>
                                <Label for="descr">
                                    Description
                                </Label>
                                <Input
                                    id="description"
                                    name="description"
                                    type="textarea"
                                    value={formData.descr}
                                    onChange={(e) => onChange(e)}
                                />
                                <Label for="descr">
                                    Car Foto
                                </Label>
                                <Input
                                    id="carfoto"
                                    name="carfoto"
                                    type="file"
                                    value={formData.carfoto}
                                    onChange={fileSelectedHandler} 
                                    accept="image/*"
                                />
                                <Label for="descr">
                                    Document
                                </Label>
                                <Input
                                    id="document"
                                    name="document"
                                    type="file"
                                    value={formData.document}
                                    onChange={(e) => onChange(e)}
                                    
                                />
                            </CardBody>
                        </Card>
                    </FormGroup>
                </Form>
            </Modal>
        </Fragment>
        );
}

export default ReturnForm;