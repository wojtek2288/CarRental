import React, { Fragment, useEffect, useState } from 'react';
import { Container, Form, FormGroup, Label, Input, Button, Card, CardBody, CardTitle, Alert } from 'reactstrap';
import { withRouter, Redirect } from 'react-router-dom';
import axios from 'axios';
import NavMenu from './NavMenu';

const SignUp = (props) => {
    const [loading, setLoading] = useState(true);
    const [userInfo, setUserInfo] = useState({
        authId: localStorage.getItem('googleId'),
        dateOfBirth: new Date(),
        driversLicenseDate: new Date(),
        email: localStorage.getItem('email'),
        location: ''
    });

    const [displayAlert, setDisplayAlert] = useState("none");

    const { authID, dateOfBirth, driversLicenseDate, email, location } = userInfo;

    const onChange = (e) => {
        console.log(e.target);
        setUserInfo({ ...userInfo, [e.target.name]: e.target.value });
    }

    const addUser = async () => {
        console.log(userInfo);
        try {
            const config = {
                headers: {
                    'Content-Type': 'application/json',
                },
            }

            const res = await axios.post('/users/clients', userInfo, config);
            console.log(res);
            setLoading(false);
        }
        catch (error) {
            console.log(error.response);
            if (error.response.status === 503) {
                setDisplayAlert("failure");
                setTimeout(removeAlert, 3000);
            }
        }
    }

    const removeAlert = () => {
        setDisplayAlert("none");
    }

    if (loading === false) {
        if (localStorage.role==='User') props.history.push('/user');
        if (localStorage.role==='Admin') props.history.push('/admin');
        else props.history.push('/');
    }

    return (      
        <Fragment>
            <NavMenu button='hide'/>
            <Container className='margin-top'>
                <h1 className='form-input'>Please provide additional info to continue</h1>
                {displayAlert === "failure" ? <Alert color="danger" className='alert-margin'>User already exists</Alert> : null}
                <Form onSubmit={(e) => {
                    e.preventDefault();
                    addUser();
                }}>
                    <FormGroup>
                        <Card className='margin-bottom'>
                            <CardBody>
                                <CardTitle tag="h5">
                                    Driver's Information
                                </CardTitle>

                                <Label for="licenceDate">
                                    Date of getting a driver license
                                </Label>
                                <Input id="licenceDate"
                                    name="driversLicenseDate"
                                    placeholder="date placeholder"
                                    type="date"
                                    value={driversLicenseDate}
                                    onChange={(e) => onChange(e)}
                                    required
                                />
                                <Label for="birthday">
                                    Birth date
                                </Label>
                                <Input id="birthday"
                                    name="dateOfBirth"
                                    placeholder="date placeholder"
                                    type="date"
                                    value={dateOfBirth}
                                    onChange={(e) => onChange(e)}
                                    required
                                />
                                <Label for="location">
                                    Location
                                </Label>
                                <Input className='form-input'
                                    id="location"
                                    name="location"
                                    placeholder="Location"
                                    type="text"
                                    value={location}
                                    onChange={(e) => onChange(e)}
                                    required
                                />
                                <Button color='primary' className='btn-text-home' type="submit">
                                    Submit
                                </Button>
                            </CardBody>
                        </Card>
                    </FormGroup>
                </Form>
            </Container>
        </Fragment>
    );
};

export default withRouter(SignUp);