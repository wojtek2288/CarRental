import React from 'react';
import { Container, Form, FormGroup, Label, Input, Button, Card, CardBody, CardTitle } from 'reactstrap';

const SignUp = () => {
    return (
        <Container className='margin-top'>
            <h1 className='form-input'>Please provide additional info to continue</h1>
            <Form>
                <FormGroup>
                    <Card className='margin-bottom'>
                        <CardBody>
                            <CardTitle tag="h5">
                                Driver's Information
                            </CardTitle>
                            <Label for="driverLicenseYears">
                                Years of having a driver license
                            </Label>
                            <Input className='form-input'
                                id="driverLicenseYears"
                                name="driverLicence"
                                placeholder="Years of having a driver license"
                                type="number"
                                min="0"
                            />
                            <Label for="age">
                                Age
                            </Label>
                            <Input className='form-input'
                                id="age"
                                name="age"
                                placeholder="Age"
                                type="number"
                                min="0"
                            />
                            <Label for="location">
                                Location
                            </Label>
                            <Input className='form-input'
                                id="location"
                                name="location"
                                placeholder="Location"
                                type="text"
                            />
                            <Button color='primary' className='btn-text-home'>
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