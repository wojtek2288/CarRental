import React, { Fragment } from 'react';
import { useState } from 'react';
import { Button } from 'reactstrap';
import RentModal from './RentModal';

export default function CarTable({ data }) {
    const [clicked, setClicked] = useState({ state: false, id: 0 })
    const [choosenCar, setChoosenCar] = useState({});
    const [modalIsOpen, setIsOpen] = React.useState(false);

    const openModal = () => {
        setIsOpen(true);
    }

    const closeModal = () => {
        setIsOpen(false);
    }

    return (
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Company</th>
                    <th>Brand</th>
                    <th>Model</th>
                    <th>Details</th>
                </tr>
            </thead>
            <tbody>
                {data.map(car =>
                    <tr key={car.id}>
                        <td>Car Rental</td>
                        <td>{car.brand}</td>
                        <td>{car.model}</td>
                        <td>
                            <Button id="rent-me" onClick={() => {
                                if (car.id !== clicked.id)
                                    setClicked({ state: true, id: car.id });
                                else
                                    setClicked({ state: !clicked.state, id: car.id });

                                setChoosenCar(car);
                            }}
                                outline color="primary" type="button">See Details
                            </Button>
                            <div id='details' hidden={!clicked.state || car.id != clicked.id}>
                                <b>Horse Power:</b>
                                <p>{car.horsepower}</p>
                                <b>Year of Production:</b>
                                <p>{car.yearOfProduction}</p>
                                <b>Description:</b>
                                <p>{car.description}</p>
                                <Button id='openModal-btn' color='primary' onClick={() => openModal()}>Check Price</Button>
                                <RentModal id='modal' modalIsOpen={modalIsOpen} closeModal={closeModal} choosenCar={choosenCar}/>
                            </div>
                        </td>
                    </tr>
                )}
            </tbody>
        </table>)
}