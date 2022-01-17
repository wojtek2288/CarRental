import React, { Fragment, useState, useEffect } from 'react';
import { CardBody, CardTitle, Container, Card, Button } from 'reactstrap';
import NavMenu from './NavMenu';
//import ReturnForm from './ReturnModal';
import ReturnForm from './ReturnModal copy';
import ReturnData from './Download';
import axios from 'axios';

const RentedCars = (props) => {
    
    const url = props.role === 'User' ? props.url + '/' + localStorage.getItem('googleId') : props.url;

    const [data, setData] = useState([]);
    const [clicked, setClicked] = useState({ state: false, id: 0 })
    const [choosenHist, setChoosenHist] = useState({});
    const [modalIsOpen, setIsOpen] = React.useState(false);
    const [refresh, setRefresh] = useState(false);

    function fetchData() {

        fetch(url)
            .then((response) => response.json())
            .then((json) => setData(json))
    }

    const openModal = () => {
        setIsOpen(true);
    }

    const closeModal = () => {
        setIsOpen(false);
    }

    useEffect(() => {
        fetchData();
    }, [refresh])

    console.log(data);

    const getDate = (hist) => {
        return new Date(hist.year, hist.month-1, hist.day);
    }

    const today = new Date();

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
                                <th>{props.url === '/rentals/hist' ? "Details" : "Action"}</th>
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
                                            </div>

                                            <ReturnData
                                            refresh={setRefresh}
                                                hist={hist}
                                                hidden={
                                                    props.url === '/rentals/curr' ||
                                                    !clicked.state || hist.id != clicked.id
                                                }
                                            >
                                            </ReturnData>
                                        </td>
                                        :
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
                                            </div>

                                            <Button hidden={props.url === '/rentals/hist'} color='primary'
                                                
                                                onClick={() =>
                                                {
                                                    openModal();
                                                    setChoosenHist(hist);
                                                }
                                            }>Return</Button>

                                            <ReturnForm hidden={props.url === '/rentals/hist'}
                                                modalIsOpen={modalIsOpen}
                                                closeModal={closeModal}
                                                choosenHist={choosenHist}
                                                setChoosenHist={setChoosenHist}
                                                refresh={setRefresh}
                                            />
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
/*disabled={today.getDate() !== hist.day || today.getDate()+1 !== hist.month || today.getYear() !== hist.year}*/ 