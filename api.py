#Fabio Leandro Lapuinka
#Cassyo Junior         
#Equipe SAMMET  12/10/2017
#Rest API - MongoDB - BigData - Telemetria - versão 1
import unicodedata
import string
import re
import sys  
reload(sys)  
sys.setdefaultencoding('utf8')

def remove_accents(data):
    return data.encode('utf-8').strip()
    #return ''.join(x for x in unicodedata.normalize('NFKD', data) if x in string.ascii_letters).lower()


#curl -d "token=value1&json=1" -X POST http://127.0.0.1:8000/DoutrinaAgil/api/find/
#curl -d "token=DAWbiMVyDhNOhBOgs7vbFMhEIUrLSQ6o2FZea=&query=Association" -X POST https://lapuinka.pythonanywhere.com/DoutrinaAgil/dev/find/
@request.restful()
def find():
    response.view = 'generic.json'
    response.headers["Access-Control-Allow-Origin"] = '*'
    response.headers['Access-Control-Max-Age'] = 86400
    response.headers['Access-Control-Allow-Headers'] = '*'
    response.headers['Access-Control-Allow-Methods'] = '*'
    response.headers['Access-Control-Allow-Credentials'] = 'true'
    def GET2(*args,**vars):
        patterns = 'auto'
        parser = db.parse_as_rest(patterns,args,vars)
        if parser.status == 200:
            return dict(content=parser.response)
        else:
            raise HTTP(parser.status,parser.error)
    def GET():
        return POST(token,query)
    def POST(token,query):
        total  = 3
        v= "2"
        from datetime import datetime
        agora = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        a = request.env.http_x_forwarded_for

        if str(token) != "DAWbiMVyDhNOhBOgs7vbFMhEIUrLSQ6o2FZea=":
            return "Invalid Demo("+token+")"

        if len(str(query))<3 :
            return "Invalide Query Size, minimum 3 chars!"

        uri = "mongodb://USER:PWD@ds015942.mlab.com:15942/doutrina_agil"
        client = MongoClient(uri)
        db2 = client.get_default_database()
        livro = 1
        
        
        query = str(remove_accents(unicode(query, "utf-8")))
        books = db2['books']
        doutrinas = db2['doutrinas']

        erros = "0"
        
        
        items = re.findall(r'\S+', query)
        items = list(set(items))  
        retorno = ""
    
        try:
            acessos = db2['acessos']
            acessos.insert({'v': v , 'ip': a , 'query': query, 'time': agora})
        except pymongo.errors.ConnectionFailure:
            erros="1"

        mylist = []
        cursorBooks = books.find()
        query = ""
        for item in items:
            virgula = ""
            if len(str(item))<3 :
                continue
            query = query + " " + item    
            cursor = doutrinas.find({'texto' : {'$regex' : '' + item +'', '$options': 'i'}});
            for doc in cursor:
                if doc['_id'] in mylist:
                    continue
                mylist.append(doc['_id'])    
                retorno  = retorno + (virgula + '{"book_id":"%s" , "page":"%s", "texto":"%s"}' %
                  (doc['book_id'], doc['page'], doc['texto']))
                virgula = ","

        
        livros = ""
        virgula = ""
        #Livros
        for doc in cursorBooks:
            livros  = livros + (virgula + '{"book_id":"%s" , "author":"%s", "title":"%s"}' %
                    (doc['id'], doc['author'], doc['title']))
            virgula = ","
        total = 0
        virgula = ""

        if retorno == "":
            livros = ""
        try:
            estatisticas = db2['estatisticas']
            estatisticas.insert({'v': v , 'ip': a , 'query': query, 'livros': livros ,'time': agora})
            total = estatisticas.count()
        except pymongo.errors.ConnectionFailure:
            erros="1"

        est = str(total)    
        
        if retorno == "":
            return '{}'
            
        return '{"livros":['+livros+'], "doutrinas":[' + retorno + '],"q":"'+query+'", "v":"'+v+'","n":"'+agora+'","a":"'+a+'","t":"'+est+'"}'


    return dict(GET=GET, POST=POST)
