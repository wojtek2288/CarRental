import React, { Fragment, useState, useEffect } from 'react';
import { CardBody, CardTitle, Container, Card, Button } from 'reactstrap';
import NavMenu from './NavMenu';
import ReturnForm from './ReturnModal';

const RentedCars = (props) => {
    const [data, setData] = useState([]);
    const [clicked, setClicked] = useState({ state: false, id: 0 })
    const [choosenHist, setChoosenHist] = useState({});
    const [modalIsOpen, setIsOpen] = React.useState(false);

    console.log(props);
    console.log(props.role === 'User' ?  props.url+ '/' + localStorage.getItem('googleId') : props.url);

    const openModal = () => {
        setIsOpen(true);
    }

    const closeModal = () => {
        setIsOpen(false);
    }

    useEffect(() => {
        // const url = '/rentals/details';
        const url =  props.role === 'User' ?  props.url+ '/' + localStorage.getItem('googleId') : props.url;
        fetch(url)
            .then((response) => response.json())
            .then((json) => setData(json));
    }, [])

    const getDate = (hist) => {
        return new Date(hist.year, hist.month, hist.day);
    }

    return (<Fragment>
        <NavMenu logged={true} />
        <Container className='margin-top'>
            <Card>
                <CardBody>
                    <CardTitle tag='h5'>{props.title}</CardTitle>
                    <table id="cars">
                        <thead>
                            <tr>
                                <th>Brand</th>
                                <th>Model</th>
                                <th>{props.url === '/hist/Rented' ? "Details" : "Action"}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {data.map(hist =>
                                <tr key={hist.id}>
                                    <td>{hist.brand}</td>
                                    <td>{hist.model}</td>
                                    {props.role === 'User' ?
                                        <td>
                                            <Button id="rent_me" onClick={() => {
                                                if (hist.id !== clicked.id)
                                                    setClicked({ state: true, id: hist.id });
                                                else
                                                    setClicked({ state: !clicked.state, id: hist.id });
                                            }}
                                                outline color="primary" type="button">See Details
                                            </Button>
                                            <div hidden={!clicked.state || hist.id != clicked.id}>
                                                <b>Company:</b> <p>CarRental</p>
                                                <b>Return Date:</b> <p>{getDate(hist).toDateString()}</p>
                                                <b>Note:</b> <p>{hist.note}</p>
                                                <Button color="primary" size="sm">💾 Download</Button><br/>
                                                <Button color="secondary" size="sm">💾 Download</Button>
                                            </div>
                                        </td>
                                        :
                                        <td>
                                            <Button hidden={props.url === '/hist/Renting'} id="rent_me" outline color="primary" type="button">See Details?</Button>
                                            <Button hidden={props.url === '/hist/Rented'} color='primary' onClick={() =>
                                                {
                                                    openModal();
                                                    setChoosenHist(hist);
                                                }
                                            }>Return</Button>
                                            <ReturnForm hidden={props.url === '/hist/Rented'} modalIsOpen={modalIsOpen} closeModal={closeModal} choosenCar={choosenHist} />
                                        </td>
                                    }
                                </tr>
                            )}
                        </tbody>
                    </table>
                </CardBody>
            </Card>
        </Container>
    </Fragment>)
}

export default RentedCars;