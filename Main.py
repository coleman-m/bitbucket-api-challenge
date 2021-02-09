import urllib.request as rq
import json
import datetime

def parse_datetime(dt: str) -> datetime.datetime:
    return datetime.datetime.strptime(dt, '%Y-%m-%dT%H:%M:%S%z')

def sort_datetime(val: tuple) -> datetime.datetime:
    return parse_datetime(val[0])

def do_request(url: str, method: str) -> json:
    request = rq.Request(url, method=method)
    response = rq.urlopen(request)
    response_data = json.loads(response.read())
    response.close()
    return response_data

def extract_timestamp_message_array(obj: [json]) -> [(datetime.datetime, str)]:
    result = list()
    for index in range(len(obj)):
        result.append((obj[index]['date'], obj[index]['message']))
    return result

if __name__ == '__main__':
    response_obj = do_request('https://bitbucket.org/api/2.0/repositories/calanceus/interviews17/commits', 'GET')
    arr = extract_timestamp_message_array(response_obj['values'])
    arr.sort(key=sort_datetime, reverse=True)
    for i in range(len(arr)):
        print(arr[i])