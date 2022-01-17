import React from 'react';
import { mount, shallow } from 'enzyme';
import CarTable from '../components/CarTable';

describe("CarTable", () => {
    const data = [];
    data.push({
        id: '1234',
        brand: 'Audi',
        model: 'A4',
        horsepower: 300,
        yearOfProduction: 2006,
        description: 'Cool audi'
    });

    test("Renders without crashing", () => {
        shallow(<CarTable data={data} />);
    });

    test("Shows description on click", () => {
        const wrapper = shallow(<CarTable data={data} />);
        wrapper.find('#rent-me').simulate('click').at(0);
        const div = wrapper.find('#details');
        const hidden = div.prop('hidden');
        expect(hidden).toBe(false);
    });

    test("Opens modal on Rent Me click", () => {
        const wrapper = mount(<CarTable data={data} />);
        wrapper.find('#rent-me').at(0).simulate('click');
        wrapper.find('#openModal-btn').at(0).simulate('click');
        const modal = wrapper.find('#modal');
        const isOpen = modal.prop('modalIsOpen');
        expect(isOpen).toBe(true);
    });
});
