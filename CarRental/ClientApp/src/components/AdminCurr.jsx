import React from 'react';
import RentedCars from './CarTableRented';

const UserRented = () => {
    return (<RentedCars url='/rentals/curr' role='Admin' title = 'Currently Rented Cars'/>);
    // return (<RentedCars url='/hist/Renting' role='Admin' title = 'Currently Rented Cars'/>);
}
export default UserRented;