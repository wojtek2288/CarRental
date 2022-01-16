import React, { Fragment, useState } from 'react';
import { Button, FormGroup, Card, CardBody, CardTitle, Input, Form, Label, Alert, Spinner} from 'reactstrap';
import Modal from 'react-modal';
import axios from 'axios';

const ReturnForm = (props) => {

    // useEffect(() => {    
    //     console.log(props.choosenHist);
    // }, []);

    let carstates = ['Good','Average','Bad'];
    const [noteData, setNoteData] = useState({
        odometer: 0,
        description: '',
        carstate: 'Good'
    });
    const [alertType, setAlertType] = useState('')
    const [alertText, setAlertText] = useState('');
    const [displayAlert, setDisplayAlert] = useState(false);

    const close = () =>{
        props.closeModal();
    }

    const [selectedImage, setSelectedImage] = useState(null);
    const [selectedDocument, setSelectedDocument] = useState(null);
    const imageSelectedHandler = (event) => {
        setSelectedImage(event.target.files[0]);
        //console.log(selectedImage);
    }
    const documentSelectedHandler = (event) => {
        setSelectedDocument(event.target.files[0]);
        //console.log(selectedDocument);
    }

    const updateRental = async () => {
        if (selectedImage!==null && selectedDocument!==null){

            const updated = props.choosenHist;
            updated.returned = true;
            updated.imagename = selectedImage.name;
            updated.documentname = selectedDocument.name;
            updated.note = 'The overall state is ' + noteData.carstate.toLowerCase() + '.\n'
                        + 'The odometer equals '+ noteData.odometer +'.\n' + noteData.description;
            
            props.setChoosenHist(updated);
            const url = '/rentals/return/' + updated.id;

            if (updated !== undefined && updated.id !== undefined && updated.id!==0) {
                try {
                    const config = {
                        headers: {
                            'Content-Type': 'application/json',
                        },
                    }
                    const res = await axios.patch(url, updated, config);
                    //console.log(res);
                    setDisplayAlert(true);
                    setTimeout(close, 3000);
                    props.refresh(true);
                    setAlertType('success');
                    setAlertText('Successfully Returned Car');
                }
                catch (error) {
                    console.log(error);
                    setDisplayAlert(true);
                    setAlertType('danger');
                    setAlertText('Cannot return it');
                }
            }
            else {
                console.log('Zle przekazane choosenHist');
                console.log(props.choosenHist);
                
            }
        }
        else console.log('Nie zaznaczone pliki');
    }

    const imageUploadHandler = async () => {
        const fd = new FormData();
        fd.append('image', selectedImage, selectedImage.name);
        try
        {
            const save = await axios.post('/File/save', fd);
            //console.log(save);
        }
        catch (err) { console.log(err);}
        setSelectedImage(null);
    }
    const documentUploadHandler = async () => {
        const fd = new FormData();
        fd.append('document', selectedDocument, selectedDocument.name);
        try
        {
            const save = await axios.post('/File/save', fd);
            //console.log(save);
        }
        catch (err) { console.log(err);}
        setSelectedDocument(null);
    }

    const onChange = (e) => {
        setNoteData({ ...noteData, [e.target.name]: e.target.value });
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
                <Form onSubmit={(e) => {
                    e.preventDefault();
                    updateRental();
                }}>
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
                                    value={noteData.odometer}
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
                                    value={noteData.carstate}
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
                                    value={noteData.descr}
                                    onChange={(e) => onChange(e)}
                                />
                                <br/>
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
                                <br/>
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
                                <br/>
                                <Button className='btn-text-home' color='success' type="submit">
                                    Return Car
                                </Button>
                                <br/>
                            </CardBody>
                        </Card>
                        {displayAlert ? <Alert color={alertType}>{alertText}</Alert> : null}
                    </FormGroup>
                </Form>
            </Modal>
        </Fragment>
        );
}

export default ReturnForm;