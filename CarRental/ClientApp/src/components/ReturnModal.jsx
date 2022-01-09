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

    const close = () =>{
        props.closeModal();
    }

    const [selectedImage, setSelectedImage] = useState(null);
    const [selectedDocument, setSelectedDocument] = useState(null);
    const imageSelectedHandler = (event) => { setSelectedImage(event.target.files[0]);}
    const documentSelectedHandler = (event) => { setSelectedDocument(event.target.files[0]);}

    const imageUploadHandler = async () => {
        const fd = new FormData();
        fd.append('image', selectedImage, selectedImage.name);
        try
        {
            const res = await axios.post('/File/save', fd);
            console.log(res);
        }
        catch (err) { console.log(err);}
        setSelectedImage(null);
    }
    const documentUploadHandler = async () => {
        const fd = new FormData();
        fd.append('document', selectedDocument, selectedDocument.name);
        try
        {
            const res = await axios.post('/File/save', fd);
            console.log(res);
        }
        catch (err) { console.log(err);}
        setSelectedDocument(null);
    }

    const onChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
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
                                    Car Picture
                                </Label>
                                <Input
                                    id="carfoto"
                                    name="carfoto"
                                    type="file"
                                    onChange={imageSelectedHandler} 
                                    accept="image/*"
                                />
                                <button hidden={selectedImage===null} type='button' onClick={imageUploadHandler}>Upload</button>
                                <Label for="descr">
                                    Document
                                </Label>
                                <Input
                                    id="document"
                                    name="document"
                                    type="file"
                                    onChange={documentSelectedHandler}
                                    accept="document/*"
                                />
                                <button hidden={selectedDocument===null} type='button' onClick={documentUploadHandler}>Upload</button>
                            </CardBody>
                        </Card>
                    </FormGroup>
                </Form>
            </Modal>
        </Fragment>
        );
}

export default ReturnForm;