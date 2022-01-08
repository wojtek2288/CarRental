import React from 'react';
import { useState, useEffect, Fragment } from 'react';
import { Button } from 'reactstrap';
import RentModal from './RentModal';
import './CarTableStyles.css';

const CarTable = (props) => {
    const [clicked, setClicked] = useState({ state: false, id: 0 })
    const [choosenCar, setChoosenCar] = useState({});
    const [modalIsOpen, setIsOpen] = React.useState(false);

    useEffect(() => {
        console.log(choosenCar);
    }, [choosenCar]);

    const openModal = () => {
        setIsOpen(true);
    }

    const closeModal = () => {
        setIsOpen(false);
    }

    return (
        <Fragment>
            <table id="cars">
                <thead>
                    <tr>
                        <th>Brand</th>
                        <th>Model</th>
                        <th>Available At</th>
                    </tr>
                </thead>
                <tbody>
                    {props.data.map(group =>
                        <tr key={group[0].brand + ' ' + group[0].model} >
                            <td>{group[0].brand}</td>
                            <td>{group[0].model}</td>
                            <td>
                                <table id="car-list">
                                    <tbody>
                                        {group.map(car =>
                                            <tr key={car.id}>
                                                <td>CarRental</td>
                                                <td>
                                                    <Button id="rent_me" onClick={() => {
                                                        if (car.id !== clicked.id)
                                                            setClicked({ state: true, id: car.id });
                                                        else
                                                            setClicked({ state: !clicked.state, id: car.id });
                                                    }}
                                                        outline color="primary" type="button">See Details
                                                    </Button>
                                                    <Button hidden={props.role === 'Admin'}
                                                        onClick={() =>
                                                            {
                                                                openModal();
                                                                setChoosenCar(car);
                                                            }
                                                        }
                                                        color='primary'>Check Price</Button>
                                                    <RentModal hidden={props.role === 'Admin'} modalIsOpen={modalIsOpen} closeModal={closeModal} choosenCar={choosenCar} />

                                                    <div hidden={!clicked.state || car.id != clicked.id}>
                                                        <p>
                                                            Horse Power: <br /> <i>{car.horsepower}</i> <br />
                                                            Year of Production: <br /> <i>{car.yearOfProduction}</i> <br />
                                                            Description:<br /> <i>{car.description}</i>
                                                        </p>
                                                    </div>
                                                </td>
                                            </tr>)
                                        }
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </Fragment>)
}

export default CarTable;