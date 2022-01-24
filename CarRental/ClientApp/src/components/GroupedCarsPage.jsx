import React, { Fragment, useState, useEffect } from 'react';
import { CardTitle, Container, FormGroup, Input, Spinner } from 'reactstrap';
import CarTable from './CarTableAll';
import NavMenu from './NavMenu';
import axios from 'axios';

export default function GroupedCars() {
    const [data, setData] = useState([])
    const [loading, setLoading] = useState(true)
    const [brand, setBrand] = useState("")
    const [model, setModel] = useState("")
    const [company, setCompany] = useState("");
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

    function getbm(data) {
        var brandmod = [];
        data.forEach(
            car => {
                var key = (car[['brand']] + ' ' + car[['model']]);
                if (brandmod.find(bm => bm === key) === undefined) {
                    brandmod.push(key);
                }
            });
        return brandmod;
    }

    function search(rows) {
        return rows.filter(
            row => row[0].brand.indexOf(brand) >= 0
                && row[0].model.indexOf(model) >= 0
                && row[0].company.indexOf(company) >= 0
        );
    }

    function byBrand(row1, row2) {
        return row1[0].brand > row2[0].brand;
    }
    function byModel(row1, row2) {
        return row1[0].model > row2[0].model;
    }

    function byCompany(row1, row2) {
        return row1[0].company > row2[0].company;
    }

    function sortRows(rows) {
        var cmp;

        if (sortColumn == 'Model') {
            cmp = byModel
        }
        if (sortColumn == 'Brand') {
            cmp = byBrand
        }
        if (sortColumn == 'Company') {
            cmp = byCompany;
        }

        let tmp = [].concat(rows);
        let rownum = tmp.length;
        for (var i = 0; i < rownum; i++) {
            for (var j = 0; j < rownum; j++) {
                let r1 = tmp[i];
                let r2 = tmp[j];
                if (i != j && cmp(r2, r1)) {
                    tmp[i] = r2
                    tmp[j] = r1
                }
            }
        }
        return tmp;
    }

    function customview() {
        let brandmod = getbm(data);
        var grouped = groupBy(data, ['brand'], ['model']);
        var toMap = [];
        brandmod.forEach(bm => toMap.push(grouped[bm]));

        var res;
        let rows = toMap;
        if (sortColumn != '') {
            res = sortRows(rows);
        }
        else {
            res = search(rows)
        }
        return res;
    }

    var groupBy = function (data, key1, key2) {
        return data.reduce(function (storage, item) {
            var group = item[key1] + ' ' + item[key2];
            storage[group] = storage[group] || [];
            storage[group].push(item);
            return storage;
        }, {});
    };

    const sortable = ['', 'Company', 'Brand', 'Model'];

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
                            value={company}
                            onChange={(e => setCompany(e.target.value))}
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
                            <option value="" disabled selected hidden>Sort By</option>
                            {sortable.map((column, key) =>
                                <option key={key}>{column}</option>)}
                        </Input>
                    </FormGroup>
                </div>
            </Container>
            {loading == true ?
                (<Spinner className="center" />) :
                (<Container className='margin-top'>
                    <CarTable data={customview()} role={localStorage.role}/>
                </Container>)}
        </Fragment>
    );
}