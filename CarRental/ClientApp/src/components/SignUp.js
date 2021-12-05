import React, { useState } from 'react';
import { Container, Form, FormGroup, Label, Input, Button, Card, CardBody, CardTitle } from 'reactstrap';
import axios from 'axios';

const SignUp = () => {

    const [userInfo, setUserInfo] = useState({
        authId: localStorage.getItem('googleId'),
        dateOfBirth: new Date(),
        driversLicenseDate: new Date(),
        email: localStorage.getItem('email'),
        location: ''
    });
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
        }
        catch (err) {
            console.log(err);
        }
    }

    return (
        <Container className='margin-top'>
            <h1 className='form-input'>Please provide additional info to continue</h1>
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
    );
};

export default SignUp;