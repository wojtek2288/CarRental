import React from 'react';
import { mount } from 'enzyme';
import RentModal from '../components/RentModal';

describe("RentModal", () => {
    let modalIsOpen = false;

    const openModal = () => {
        modalIsOpen = true;
    }
    const closeModal = () => {
        modalIsOpen = false;
    }

    const choosenCar = {
        id: '1234',
        brand: 'Audi',
        model: 'A4',
        horsepower: 300,
        yearOfProduction: 2006,
        description: 'Cool audi'
    }

    test("Renders without crashing", () => {
        openModal();
        mount(<RentModal id='modal' modalIsOpen={modalIsOpen} closeModal={closeModal} choosenCar={choosenCar} />);
    });

    test("Selecting dates", () => {
        openModal();
        const wrapper = mount(<RentModal id='modal' modalIsOpen={modalIsOpen} closeModal={closeModal} choosenCar={choosenCar} />);
        const from = wrapper.find('#from').at(0);
        const to = wrapper.find('#to').at(0);

        from.simulate('change', { target: { name: 'from', value: new Date(2022, 6, 24) } });
        to.simulate('change', { target: { name: 'to', value: new Date(2022, 6, 28) } });

        expect(wrapper.find('#from').at(0).prop("value")).toStrictEqual(new Date(2022, 6, 24));
        expect(wrapper.find('#to').at(0).prop("value")).toStrictEqual(new Date(2022, 6, 28));
    });
});