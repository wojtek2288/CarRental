import React, { Fragment, useState, useEffect } from 'react';
import { CardTitle, Container, FormGroup, Input, Spinner } from 'reactstrap';
import CarTable from './CarTable'
import NavMenu from './NavMenu';
import axios from 'axios';

export default function ViewCars() {
    const [data, setData] = useState([])
    const [loading, setLoading] = useState(true);
    const [brand, setBrand] = useState("")
    const [model, setModel] = useState("")
    const [sortColumn, setSortColumn] = useState("")

    useEffect(() => {
        fetch('cars', {
            headers: {
                'ApiKey': axios.defaults.headers.common['ApiKey']
            }
        })
            .then((response) => response.json())
            .then((json) => setData(json))
            .then(() => setLoading(false));
    }, [])

    function search(rows) {
        return rows.filter(
            row => row.brand.indexOf(brand) >= 0
                && row.model.indexOf(model) >= 0
        );
    }

    function byBrand(row1, row2) {
        return row1.brand > row2.brand;
    }
    function byModel(row1, row2) {
        console.log(row1.model)
        console.log(row2.model)
        return row1.model > row2.model;
    }

    function sortRows(rows) {
        var cmp;

        if (sortColumn == 'Model') {
            cmp = byModel
            console.log(cmp)
        }
        if (sortColumn == 'Brand') {
            cmp = byBrand
        }
        if (sortColumn == 'Company') {
            cmp = (a, b) => false;
        }

        let tmp = [].concat(rows);
        let rownum = tmp.length;
        for (var i = 0; i < rownum; i++) {
            for (var j = 0; j < rownum; j++) {
                let r1 = tmp[i];
                let r2 = tmp[j];
                if (i != j && cmp(r2, r1)) {
                    console.log(r1)
                    console.log(r2)
                    tmp[i] = r2
                    tmp[j] = r1
                }
            }
        }
        return tmp;
    }

    function customview() {
        console.log(sortColumn)
        console.log(data);
        var rows = data
        var res;
        if (sortColumn != '') {
            res = sortRows(search(rows));
        }
        else {
            res = search(rows)
        }
        return res;
    }

    const sortable = ['','Company', 'Brand', 'Model'];
    const contents = <CarTable data={customview()} />

    return (
        <Fragment>
            <NavMenu logged={true} />
            <Container className='margin-top'>
                <div>
                    <CardTitle tag="h5"> Search </CardTitle>
                    <FormGroup className='search-text'>
                        <Input className='search-input'
                            id="car-company"
                            name="company"
                            placeholder="Company"
                            type="text"
                        />
                        <Input className='search-input'
                            id="car-brand"
                            name="brand"
                            placeholder="Brand"
                            type="text"
                            value={brand}
                            onChange={(e => setBrand(e.target.value))}
                        />
                        <Input className='search-input'
                            id="car-model"
                            name="model"
                            placeholder="Model"
                            type="text"
                            value={model}
                            onChange={(e => setModel(e.target.value))}
                        />
                        <Input className='search-input'
                            id="car-column"
                            name="column"
                            type="select"
                            value={sortColumn}
                            onChange={(e) => setSortColumn(e.target.value)}>
                            {sortable.map((column, key) =>
                                <option key={key}>{column}</option>)}
                        </Input>
                    </FormGroup>
                </div>
            </Container>
            {loading == true ?
            (<Spinner className="center" />) :
            (<Container className='margin-top'>
                {contents}
            </Container>)}
        </Fragment>
    );
}