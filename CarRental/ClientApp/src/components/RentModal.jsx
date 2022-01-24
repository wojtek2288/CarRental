import React, { Fragment, useState } from 'react';
import { Button, FormGroup, Card, CardBody, CardTitle, Input, Form, Label, Alert, Spinner} from 'reactstrap';
import Modal from 'react-modal';
import axios from 'axios';

const RentModal = (props) => {
    const [formData, setFormData] = useState({
        from: new Date(),
        to: new Date()
    });
    const [checkingPrice, setCheckingPrice] = useState(true);
    const [renting, setRenting] = useState(false);
    const [text, setText] = useState('Check price');
    const [alertType, setAlertType] = useState('')
    const [alertText, setAlertText] = useState('');
    const [quotaId, setquotaId] = useState('');
    const [displayAlert, setDisplayAlert] = useState(false);
    const [loading, setLoading] = useState(false);
    const [hideButton, setHideButton] = useState(false);

    const onChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    }

    const close = () => {
        setCheckingPrice(true);
        setRenting(false);
        setText('Check price');
        setquotaId('');
        setDisplayAlert(false);
        setLoading(false);
        setHideButton(false);
        setAlertType('');
        props.closeModal();
    }

    const checkPrice = async () => {
        setLoading(true);
        console.log(formData);
        const authId = localStorage.getItem('googleId');
        const carId = props.choosenCar.id;
        console.log(props.choosenCar);
        console.log(formData);

        try {
            const config = {
                headers: {
                    'Content-Type': 'application/json',
                },
            }
            const res = await axios.post('/cars/GetPrice/' + authId + '/' + carId, formData, config);
            console.log(res);
            setText("Rent now for: " + res.data.price + " " + res.data.currency);
            setCheckingPrice(false);
            setRenting(true);
            setquotaId(res.data.id);
            setLoading(false);
        }
        catch (err) {
            console.log(err);
            if (err.response.status === 500) {
                setCheckingPrice(true);
                setLoading(false);
                setDisplayAlert(true);
                setAlertType('danger');
                setAlertText('This rental company is currently not avaliable');
            }
        }
    }

    const rent = async () => {
        setLoading(true);
        try {
            const config = {
                headers: {
                    'Content-Type': 'application/json',
                },
            }
            let startDate = formData.from;
            const res = await axios.post('/cars/Rent/' + quotaId, startDate, config);
            console.log(res);
            setDisplayAlert(true);
            setTimeout(close, 3000);
            setAlertType('success');
            setAlertText('Successfully Rented Car');
            setLoading(false);
            setHideButton(true);
        }
        catch (err) {
            console.log(err);
            if (err.response.status === 400) {
                setCheckingPrice(true);
                setRenting(false);
                setquotaId('');
                setText('Check price');
                setDisplayAlert(true);
                setAlertText('Car is unavaliable at the selected dates');
                setAlertType('danger');
                setLoading(false);
            }
        }
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
                {displayAlert ? <Alert color={alertType}>{alertText}</Alert> : null}
                <Button className="closeBtn" onClick={close} close />
                <h1 className='form-input' className='margin-bottom'>Rent {props.choosenCar.brand} {props.choosenCar.model}</h1>
                <Form onSubmit={(e) => {
                    e.preventDefault();
                    if (checkingPrice)
                        checkPrice();
                    else if (renting)
                        rent();
                }}>
                    <FormGroup>
                        <Card className='margin-bottom'>
                            <CardBody>
                                <CardTitle tag="h5">
                                    Choose rental dates
                                </CardTitle>

                                <Label for="from">
                                    From
                                </Label>
                                <Input id="from"
                                    name="from"
                                    placeholder="date placeholder"
                                    type="date"
                                    value={formData.from}
                                    onChange={(e) => onChange(e)}
                                    className='margin-bottom'
                                    required
                                />
                                <Label for="to">
                                    To
                                </Label>
                                <Input id="to"
                                    name="to"
                                    placeholder="date placeholder"
                                    type="date"
                                    value={formData.to}
                                    onChange={(e) => onChange(e)}
                                    className='margin-bottom'
                                    required
                                />
                                {loading ? <Spinner /> :
                                    (
                                    <Button hidden={hideButton} color={checkingPrice === true ? 'primary' : 'success'} className='btn-text-home' type="submit">
                                        {text}
                                    </Button>
                                    )}

                            </CardBody>
                        </Card>
                    </FormGroup>
                </Form>
            </Modal>
        </Fragment>
        );
}

export default RentModal;