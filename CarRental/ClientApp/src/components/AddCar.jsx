import React, { Fragment, useState } from 'react'
import { Container, Form, FormGroup, Label, Input, Button, Card, CardBody, CardTitle, Alert } from 'reactstrap';
import NavMenu from './NavMenu';
import { postCar } from '../Utils/utils';

// Route for Car Rental Company Worker only

const AddCar = (props) => {

    let years = [];

    for (let i = 2021; i >= 1990; i--) {
        years.push(i);
    }

    const [formData, setFormData] = useState({
        brand: '',
        model: '',
        yearOfProduction: '2021',
        horsePower: '',
        description: '',
    });

    const [displayAlert, setDisplayAlert] = useState(false);

    const { brand, model, yearOfProduction, horsePower, description } = formData;

    const onChange = (e) => {
        console.log(e.target);
        setFormData({ ...formData, [e.target.name]: e.target.value });
    }

    const handleAdding = () => {
        setDisplayAlert(true);
        setTimeout(removeAlert, 3000);
        setFormData({
            brand: '',
            model: '',
            yearOfProduction: '2021',
            horsePower: '',
            description: '',
        });
    }

    const removeAlert = () => {
        setDisplayAlert(false);
    }

    return (
        <Fragment>
            <NavMenu logged={true}/>
            <Container className='margin-top'>
                <h1 className='form-input'>Add a Car</h1>
                <Alert hidden={!displayAlert} className='alert-margin'>Successfully Added Car</Alert>
                <Card className='margin-bottom'>
                    <CardBody>
                        <CardTitle tag="h5">
                            Car Details
                        </CardTitle>
                        <Form onSubmit={async (e) => {
                            e.preventDefault();
                            await postCar(formData);
                            handleAdding();
                        }}>
                            <FormGroup>
                                <Label for="brand">
                                    Car Brand
                                </Label>
                                <Input className='form-input'
                                    id="car-brand"
                                    name="brand"
                                    placeholder="Car Brand"
                                    type="text"
                                    value={brand}
                                    onChange={(e) => onChange(e)}
                                    required
                                />
                                <Label for="model">
                                    Car Model
                                </Label>
                                <Input className='form-input'
                                    id="car-model"
                                    name="model"
                                    placeholder="Car Model"
                                    type="text"
                                    value={model}
                                    onChange={(e) => onChange(e)}
                                    required
                                />
                                <Label for="yearOfProduction">
                                    Year of Production
                                </Label>
                                <Input
                                    className="form-input"
                                    id="year-of-production"
                                    name="yearOfProduction"
                                    type="select"
                                    placeholder="Year of Production"
                                    value={yearOfProduction}
                                    onChange={(e) => onChange(e)}
                                    required
                                >
                                    {years.map((year, key) => <option key={key}>{year}</option>)}
                                </Input>

                                <Label for="horsePower">
                                    Horse Power
                                </Label>
                                <Input className='form-input'
                                    id="horse-power"
                                    name="horsePower"
                                    placeholder="Horse Power"
                                    type="number"
                                    min="50"
                                    max="1500"
                                    value={horsePower}
                                    onChange={(e) => onChange(e)}
                                    required
                                />
                                <Card>
                                    <CardBody>
                                        <CardTitle tag="h5">
                                            Additional Information
                                        </CardTitle>

                                        <Label for="description">
                                            Description
                                        </Label>
                                        <Input
                                            id="description"
                                            name="description"
                                            type="textarea"
                                            value={description}
                                            onChange={(e) => onChange(e)}
                                        />

                                    </CardBody>
                                </Card>
                            </FormGroup>
                            <Button color='primary' className='btn-text-home' type='submit'>
                                Submit
                            </Button>
                        </Form>
                    </CardBody>
                </Card>
            </Container>
        </Fragment>
    );
}

export default AddCar;