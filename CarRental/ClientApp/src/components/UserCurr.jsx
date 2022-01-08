import React from 'react';
import RentedCars from './CarTableRented';

const UserRented = () => {
    return (<RentedCars url='/rentals/curr' role='User' title='My Bookings'/>);
    // return (<RentedCars url='/hist/Renting' role='User' title='My Bookings'/>);
}
export default UserRented;